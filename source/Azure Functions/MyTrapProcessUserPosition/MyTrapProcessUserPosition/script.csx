#r "System"
#r "System.Configuration"
#r "System.Core"
#r "System.Data"
#r "System.Data.DataSetExtensions"
#r "System.Net.Http"
#r "Newtonsoft.Json"
#r "System.Spatial"
#r "Microsoft.WindowsAzure.Storage"

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    try
    {
        Settings.Log = log;

        Settings.Log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

        Process();
    }
    catch (Exception e)
    {
        Settings.Log.Info(e.Message);
    }
    finally
    {
        if (Settings.connection != null)
        {
            Settings.Log.Info("Closing SQL...");
            Settings.connection.Close();
            Settings.Log.Info("SQL Closed!");
        }
    }
}

public class Settings
{
    public static TraceWriter Log;
    public const int MAX_LOOP_GET_MESSAGES = 100;
    public const int AMOUNT_MESSAGES_CHECK = 10;
    public static SqlConnection connection;
}

public static void Process()
{
    try
    {
        Settings.Log.Info("Connecting SQL...");
        Settings.connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyTrapConnectionString"].ToString());

        Settings.connection.Open();
        Settings.Log.Info("Connected SQL!");

        string storageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

        CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

        CloudQueue queue = queueClient.GetQueueReference("positionqueue");

        queue.CreateIfNotExists();

        List<CloudQueueMessage> messages = new List<CloudQueueMessage>();

        for (int cont = 0; cont < Settings.MAX_LOOP_GET_MESSAGES; cont++)
        {
            Settings.Log.Info("Getting messages queue loop:" + (cont + 1));

            var messagesLoop = queue.GetMessages(Settings.AMOUNT_MESSAGES_CHECK).ToList();

            if (messagesLoop == null || messagesLoop.Count == 0)
            {
                Settings.Log.Info("No more messages");
                break;
            }
            else
            {
                Settings.Log.Info("Messages Received:" + messagesLoop.Count);
                messages.AddRange(messagesLoop);
            }
        }

        if (messages != null && messages.Count > 0)
        {
            foreach (CloudQueueMessage message in messages)
            {
                try
                {
                    string json = message.AsString;

                    UserLocationRequest userLocation = JsonConvert.DeserializeObject<UserLocationRequest>(json);

                    List<ArmedTrap> armedTraps = ArmedTrap.ListNearTraps(userLocation);

                    ArmedTrap nearTrap = armedTraps.FirstOrDefault();

                    if (nearTrap != null && nearTrap.Distance <= 200)
                    {
                        ArmedTrap.DisarmTrap(nearTrap.Id);

                        SendApiProcessDisarmedTrap(userLocation.userId, nearTrap.Id);
                    }
                }
                catch (Exception e)
                {
                    Settings.Log.Info(e.Message);
                }

                queue.DeleteMessage(message);
            }
        }
    }
    catch (Exception e)
    {
        Settings.Log.Info(e.Message);
    }
    finally
    {
        if (Settings.connection != null)
        {
            Settings.Log.Info("Closing SQL...");
            Settings.connection.Close();
            Settings.Log.Info("SQL Closed!");
        }
    }
}

public class UserLocationRequest
{
    public string userId { get; set; }
    public float latitude { get; set; }
    public float longitude { get; set; }
}

public class ArmedTrap
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public string UserId { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string NameKey { get; set; }
    public int Distance { get; set; }

    public static List<ArmedTrap> ListNearTraps(UserLocationRequest userLocation)
    {
        List<ArmedTrap> result = new List<ArmedTrap>();

        string query = @"DECLARE @SearchLocation GEOGRAPHY = GEOGRAPHY::STGeomFromText('POINT({0} {1})', 4326);

                            WITH ARMED_TRAPS AS(
	                            SELECT
		                            AT.Id AS 'Id',
		                            AT.Date AS 'Date',
		                            AT.User_Id AS 'UserId',
		                            AT.Latitude AS 'Latitude',
		                            AT.Longitude AS 'Longitude',
		                            AT.NameKey AS 'NameKey',
		                            CAST(@SearchLocation.STDistance(GEOGRAPHY::Point(AT.Latitude , AT.Longitude, 4326)) / 1000 AS INT) AS 'DistanceMeters'
	                            FROM
		                            ArmedTraps AT
                            )

                            SELECT
	                            *
                            FROM
	                            ARMED_TRAPS
                            WHERE
	                            DistanceMeters < 500
                            ORDER BY
	                            DistanceMeters";

        query = string.Format(query, userLocation.longitude.ToString(CultureInfo.InvariantCulture), userLocation.latitude.ToString(CultureInfo.InvariantCulture), userLocation.userId);

        SqlCommand command = new SqlCommand(query, Settings.connection);

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                ArmedTrap armedTrap = new ArmedTrap();

                armedTrap.Id = reader.GetGuid(0).ToString();
                armedTrap.Date = reader.GetDateTime(1);
                armedTrap.UserId = reader.GetGuid(2).ToString();
                armedTrap.Latitude = (float)reader.GetDouble(3);
                armedTrap.Longitude = (float)reader.GetDouble(4);
                armedTrap.NameKey = reader.GetString(5);
                armedTrap.Distance = reader.GetInt32(6);

                result.Add(armedTrap);
            }
        }

        return result;
    }

    public static void DisarmTrap(string armedTrapId)
    {
        string query = string.Format("UPDATE ArmedTraps SET Disarmed = 1 WHERE Id = '{0}'", armedTrapId);

        SqlCommand command = new SqlCommand(query, Settings.connection);

        int result = command.ExecuteNonQuery();
    }

    public static void UpdateUserPoints(string userId, string nameKey)
    {
        int points = 0;

        string queryGetPoints = string.Format("SELECT Points FROM Traps WHERE NameKey = '{0}'", nameKey);

        SqlCommand commandSelect = new SqlCommand(queryGetPoints, Settings.connection);

        using (SqlDataReader reader = commandSelect.ExecuteReader())
        {
            while (reader.Read())
            {
                points = reader.GetInt32(0);
            }
        }

        if (points > 0)
        {
            string queryUpdateUser = string.Format("UPDATE U SET U.Points = (U.Points + {0}) FROM Users U WHERE U.Id = '{1}'", points, userId);

            SqlCommand commandUpdate = new SqlCommand(queryUpdateUser, Settings.connection);

            int result = commandUpdate.ExecuteNonQuery();
        }
    }
}

private static void SendApiProcessDisarmedTrap(string userId, string armedTrapId)
{
    try
    {
        string uri = string.Format(ConfigurationManager.AppSettings["ApiProcessDisarmedUri"] + "?userId={0}&armedTrapId={1}", userId, armedTrapId);

        var request = (HttpWebRequest)WebRequest.Create(uri);

        request.Method = "GET";
        request.ContentType = "application/json";

        var response = (HttpWebResponse)request.GetResponse();
        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
    }
    catch (Exception e)
    {
        Settings.Log.Info(e.Message);
    }
}