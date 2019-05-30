namespace SSO.Infrastructure.Cache
{
    using StackExchange.Redis;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICacheService
    {
        Task<long> GetIncrementAsync(string key);
        Task<bool> AddListItemAsync<T>(string key, T item);
        Task<List<T>> GetListAsync<T>(string key);
        Task<bool> DeleteAsync(string key);
        Task<bool> AddStringAsync<T>(string key, T value);
        Task<T> GetStringAsync<T>(string key);
        Lazy<ConnectionMultiplexer> GetConnection();
    }
}
