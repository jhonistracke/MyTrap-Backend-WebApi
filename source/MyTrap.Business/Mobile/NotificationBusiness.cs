using Microsoft.Azure.NotificationHubs;
using MyTrap.Business.Mobile.Contracts;
using MyTrap.Framework.Utils;
using MyTrap.Model.Enums;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyTrap.Business.Mobile
{
    public class NotificationBusiness : INotificationBusiness
    {
        private NotificationHubClient _hubClient;

        public NotificationBusiness()
        {
            try
            {
                string NOTIFICATION_HUB_CONNECTION_STRING = ConfigurationManager.AppSettings["NotificationHubConnectionString"];
                string NOTIFICATION_HUB_NAME = ConfigurationManager.AppSettings["NotificationHubName"];

                _hubClient = NotificationHubClient.CreateClientFromConnectionString(NOTIFICATION_HUB_CONNECTION_STRING, NOTIFICATION_HUB_NAME);
            }
            catch (Exception exception)
            {
                ElmahUtils.LogToElmah(exception);
            }
        }

        public async Task Register(string email, int platformId, string pushRegistrationId)
        {
            try
            {
                var registrationsCollection = await _hubClient.GetRegistrationsByTagAsync(email, 100);

                var registrations = registrationsCollection.ToList();

                if (registrations != null && registrations.Count > 0)
                {
                    foreach (var register in registrations)
                    {
                        await _hubClient.DeleteRegistrationAsync(register);
                    }
                }

                switch ((EPlatform)platformId)
                {
                    case EPlatform.ANDROID:

                        await _hubClient.CreateGcmNativeRegistrationAsync(pushRegistrationId, new string[] { email });

                        break;

                    case EPlatform.WP:

                        await _hubClient.CreateWindowsNativeRegistrationAsync(pushRegistrationId, new string[] { email });

                        break;
                }
            }
            catch (Exception exception)
            {
                ElmahUtils.LogToElmah(exception);
            }
        }

        public async Task SendNotificationTrapDisarmed(string email, string msg, bool owner, int points, string trapNameKey, double latitude, double longitude, string otherUserName, string otherUserImage)
        {
            try
            {
                var date = DateUtils.DateToString(DateTime.Now);

                var androidPayload = "{ \"data\" : {\"message\":\"" + msg + "\", \"owner\":\"" + (owner ? "1" : "0") + "\", \"points\":\"" + points + "\", \"show\":\"1\", \"trap\":\"" + trapNameKey + "\", \"lat\":\"" + latitude + "\", \"lng\":\"" + longitude + "\", \"date\":\"" + date + "\", \"userName\":\"" + otherUserName + "\", \"img\":\"" + otherUserImage + "\"}}";
                var windowsPayload = "{\"message\":\"" + msg + "\", \"owner\":" + (owner ? "1" : "0") + ", \"points\":" + points + ", \"show\":1, \"trap\":\"" + trapNameKey + "\", \"lat\":" + latitude.ToString(CultureInfo.InvariantCulture) + ", \"lng\":" + longitude.ToString(CultureInfo.InvariantCulture) + ", \"date\":\"" + date + "\", \"userName\":\"" + otherUserName + "\", \"img\":\"" + otherUserImage + "\"}";

                var notificationOutcomeAndroid = await _hubClient.SendGcmNativeNotificationAsync(androidPayload, email);

                WindowsNotification notificationWindows = new WindowsNotification(windowsPayload);

                notificationWindows.Headers.Add("X-NotificationClass", "3");
                notificationWindows.Headers.Add("X-WNS-Type", "wns/raw");

                var notificationOutcomeWindows = await _hubClient.SendNotificationAsync(notificationWindows, email);
            }
            catch (Exception exception)
            {
                ElmahUtils.LogToElmah(exception);
            }
        }
    }
}