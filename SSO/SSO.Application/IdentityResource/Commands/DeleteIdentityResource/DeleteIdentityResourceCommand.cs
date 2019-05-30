namespace SSO.Application.IdentityResource.Commands.DeleteIdentityResource
{
    using MediatR;

    public class DeleteIdentityResourceCommand : IRequest
    {
        public string Name { get; set; }
    }
}
