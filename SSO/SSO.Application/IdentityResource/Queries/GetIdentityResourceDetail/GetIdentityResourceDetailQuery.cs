namespace SSO.Application.IdentityResource.Queries.GetIdentityResourceDetail
{
    using MediatR;

    public class GetIdentityResourceDetailQuery : IRequest<IdentityResourceDetail>
    {
        public string Name { get; set; }
    }
}
