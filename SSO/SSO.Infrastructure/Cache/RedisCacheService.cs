namespace SSO.Infrastructure.Cache
{
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RedisCacheService : ICacheService
    {
        private readonly IOptions<RedisCacheSettings> _options;
        private Lazy<ConnectionMultiplexer> _lazyConnection;

        public RedisCacheService(IOptions<RedisCacheSettings> options)
        {
            _options = options;
        }

        public async Task<long> GetIncrementAsync(string key)
        {
            var connnection = GetConnection();

            var instanceKey = GetKeyWithInstanceName(key);
            var database = connnection.Value.GetDatabase(db: 1);

            var value = await database.StringGetAsync(instanceKey);

            var result = false;

            if (value.IsNull)
                result = await database.StringSetAsync(instanceKey, 0);

            var increment = await database.StringIncrementAsync(instanceKey);

            return increment;
        }

        public async Task<bool> AddListItemAsync<T>(string key, T item)
        {
            var connnection = GetConnection();

            var instanceKey = GetKeyWithInstanceName(key);
            var database = connnection.Value.GetDatabase(db: 1);

            var value = await database.ListRightPushAsync(instanceKey, JsonConvert.SerializeObject(item));

            return value > 0;
        }

        public async Task<List<T>> GetListAsync<T>(string key)
        {
            var connnection = GetConnection();

            var result = new List<T>();

            var instanceKey = GetKeyWithInstanceName(key);
            var database = connnection.Value.GetDatabase(db: 1);

            var listLength = await database.ListLengthAsync(instanceKey, CommandFlags.PreferSlave);

            for (var i = 0; i < listLength; i++)
            {
                var value = await database.ListGetByIndexAsync(instanceKey, i, CommandFlags.PreferSlave);

                result.Add(JsonConvert.DeserializeObject<T>(value));
            }

            return result;
        }

        public async Task<bool> DeleteAsync(string key)
        {
            var connnection = GetConnection();

            var instanceKey = GetKeyWithInstanceName(key);
            var database = connnection.Value.GetDatabase(db: 1);

            var value = await database.KeyDeleteAsync(instanceKey);

            return value;
        }

        public async Task<bool> AddStringAsync<T>(string key, T value)
        {
            var connnection = GetConnection();

            var instanceKey = GetKeyWithInstanceName(key);
            var database = connnection.Value.GetDatabase(db: 1);

            var result = await database.StringSetAsync(instanceKey, JsonConvert.SerializeObject(value));

            return result;
        }

        public async Task<T> GetStringAsync<T>(string key)
        {
            var connnection = GetConnection();

            var instanceKey = GetKeyWithInstanceName(key);
            var database = connnection.Value.GetDatabase(db: 1);

            var result = await database.StringGetAsync(instanceKey);

            return JsonConvert.DeserializeObject<T>(result);
        }

        #region Helpers

        public Lazy<ConnectionMultiplexer> GetConnection()
        {
            if (_lazyConnection == null || _lazyConnection.Value == null || !_lazyConnection.Value.IsConnected)
            {
                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints = { { _options.Value.Master }, { _options.Value.Slave } },
                    AllowAdmin = true,
                    AbortOnConnectFail = false,
                    Ssl = false
                };

                _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
            }

            return _lazyConnection;
        }

        private string GetKeyWithInstanceName(string key)
        {
            return _options.Value.InstanceName + key;
        }

        #endregion
    }
}
