using MyTrap.Framework.Utils;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace MyTrap.Repository.Cache
{
    public class RedisCache
    {
        ConnectionMultiplexer _redisServer = null;

        private async Task<ConnectionMultiplexer> GetRedisConnectionAsync()
        {
            if (_redisServer == null || !_redisServer.IsConnected)
            {
                string redisServer = ConfigurationManager.AppSettings["RedisServer"].ToString();

                _redisServer = await ConnectionMultiplexer.ConnectAsync(redisServer);
            }

            return _redisServer;
        }

        public void Close()
        {
            try
            {
                if (_redisServer != null)
                {
                    _redisServer.Close();
                }
            }
            catch { }
        }

        private ConnectionMultiplexer GetRedisConnection()
        {
            if (_redisServer == null || !_redisServer.IsConnected)
            {
                string redisServer = ConfigurationManager.AppSettings["RedisServer"].ToString();

                _redisServer = ConnectionMultiplexer.Connect(redisServer);
            }

            return _redisServer;
        }

        public async Task<bool> SetValueAsync(string key, string value, int hours = 0)
        {
            bool result = false;

            try
            {
                TimeSpan? expiration = null;

                if (hours > 0)
                {
                    expiration = TimeSpan.FromHours(hours);
                }

                ConnectionMultiplexer redisConnection = await GetRedisConnectionAsync();

                result = await redisConnection.GetDatabase().StringSetAsync(key, value, expiration);
            }
            catch (Exception exception)
            {
                ElmahUtils.LogToElmah(exception);

                result = false;
            }

            return result;
        }

        public async Task<string> GetValueAsync(string key)
        {
            string result = null;

            try
            {
                ConnectionMultiplexer redisConnection = await GetRedisConnectionAsync();

                result = await redisConnection.GetDatabase().StringGetAsync(key);
            }
            catch (Exception exception)
            {
                ElmahUtils.LogToElmah(exception);

                result = null;
            }

            return result;
        }

        public string GetValue(string key)
        {
            string result = null;

            try
            {
                ConnectionMultiplexer redisConnection = GetRedisConnection();

                result = redisConnection.GetDatabase().StringGet(key);
            }
            catch (Exception exception)
            {
                ElmahUtils.LogToElmah(exception);

                result = null;
            }

            return result;
        }

        public async Task<bool> RemoveKeyAsync(string key)
        {
            bool result = false;

            try
            {
                ConnectionMultiplexer redisConnection = await GetRedisConnectionAsync();

                result = await redisConnection.GetDatabase().KeyDeleteAsync(key);
            }
            catch (Exception exception)
            {
                ElmahUtils.LogToElmah(exception);

                result = false;
            }

            return result;
        }
    }
}