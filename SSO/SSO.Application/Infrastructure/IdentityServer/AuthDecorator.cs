namespace SSO.Application.Infrastructure.IdentityServer
{
    using System;

    internal class AuthDecorator<TService>
    {
        public TService Instance { get; set; }

        public AuthDecorator(TService instance)
        {
            Instance = instance;
        }
    }

    internal class AuthDecorator<TService, TImpl> : AuthDecorator<TService> where TImpl : class, TService
    {
        public AuthDecorator(TImpl instance) : base(instance)
        {
        }
    }

    internal class DisposableAuthDecorator<TService> : AuthDecorator<TService>, IDisposable
    {
        public DisposableAuthDecorator(TService instance) : base(instance)
        {
        }

        public void Dispose()
        {
            (Instance as IDisposable)?.Dispose();
        }
    }
}
