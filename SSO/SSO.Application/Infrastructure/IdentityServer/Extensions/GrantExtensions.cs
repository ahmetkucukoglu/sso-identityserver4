namespace SSO.Application.Infrastructure.IdentityServer.Extensions
{
    using Domain.Entities;
    using IdentityServer4.Models;

    public static class GrantExtensions
    {
        public static void UpdateGrant(this PersistedGrant persistedGrant, Grant grant)
        {
            grant.Key = persistedGrant.Key;
            grant.ClientId = persistedGrant.ClientId;
            grant.CreationTime = persistedGrant.CreationTime;
            grant.Data = persistedGrant.Data;
            grant.Expiration = persistedGrant.Expiration;
            grant.SubjectId = persistedGrant.SubjectId;
            grant.Type = persistedGrant.Type;
        }

        public static Grant CreateGrant(this PersistedGrant persistedGrant)
        {
            return new Grant
            {
                Key = persistedGrant.Key,
                ClientId = persistedGrant.ClientId,
                CreationTime = persistedGrant.CreationTime,
                Data = persistedGrant.Data,
                Expiration = persistedGrant.Expiration,
                SubjectId = persistedGrant.SubjectId,
                Type = persistedGrant.Type
            };
        }

        public static PersistedGrant GetPersistedGrant(this Grant grant)
        {
            return new PersistedGrant
            {
                Key = grant.Key,
                ClientId = grant.ClientId,
                CreationTime = grant.CreationTime,
                Data = grant.Data,
                Expiration = grant.Expiration,
                SubjectId = grant.SubjectId,
                Type = grant.Type
            };
        }
    }
}
