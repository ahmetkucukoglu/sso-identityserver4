namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceEditViewModel
{
    using Commands.UpdateIdentityResource;
    using MediatR;

    public class IdentityResourceEditViewModelQuery : IRequest<IdentityResourceEditViewModel>
    {
        public string Name { get; set; }
        public UpdateIdentityResourceCommand Command { get; set; }
    }
}
