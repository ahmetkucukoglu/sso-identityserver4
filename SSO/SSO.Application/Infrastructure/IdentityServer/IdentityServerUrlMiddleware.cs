namespace SSO.Application.Infrastructure.IdentityServer
{
    using IdentityServer4.Extensions;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class IdentityServerUrlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _url;

        public IdentityServerUrlMiddleware(RequestDelegate next, string url)
        {
            _url = url;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            context.SetIdentityServerOrigin(_url);
            context.SetIdentityServerBasePath(request.PathBase.Value.TrimEnd('/'));

            await _next(context);
        }
    }
}
