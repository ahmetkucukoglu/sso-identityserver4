namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceCreateViewModel
{
    using Commands.CreateIdentityResource;
    using MediatR;

    public class IdentityResourceCreateViewModelQuery : IRequest<IdentityResourceCreateViewModel>
    {
        public CreateIdentityResourceCommand Command { get; set; }
    }
}
