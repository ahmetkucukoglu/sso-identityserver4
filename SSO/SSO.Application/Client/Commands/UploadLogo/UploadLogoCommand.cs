namespace SSO.Application.Client.Commands.UploadLogo
{
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class UploadLogoCommand : IRequest
    {
        public string Id { get; set; }
        public IFormFile LogoFile { get; set; }
    }
}
