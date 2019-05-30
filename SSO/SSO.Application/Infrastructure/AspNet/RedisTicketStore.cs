namespace SSO.Application.Infrastructure.AspNet
{
    using SSO.Infrastructure.Cache;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using System;
    using System.Threading.Tasks;

    public class RedisTicketStore : ITicketStore
    {
        private const string PREFIX = "Ticket";
        private ICacheService _cacheService;

        public RedisTicketStore(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task RemoveAsync(string key)
        {
            await _cacheService.DeleteAsync(key);
        }

        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            await _cacheService.AddStringAsync(key, SerializeToBytes(ticket));
        }

        public async Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var ticket = await _cacheService.GetStringAsync<byte[]>(key);
            var result = DeserializeFromBytes(ticket);

            return result;
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = $"{PREFIX}{Guid.NewGuid().ToString()}";

            await RenewAsync(key, ticket);

            return key;
        }

        #region Helpers

        private byte[] SerializeToBytes(AuthenticationTicket source)
        {
            return TicketSerializer.Default.Serialize(source);
        }

        private AuthenticationTicket DeserializeFromBytes(byte[] source)
        {
            return source == null ? null : TicketSerializer.Default.Deserialize(source);
        }

        #endregion
    }
}
