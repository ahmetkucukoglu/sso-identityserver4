namespace SSO.Application.ApiResource.Queries.GetApiResourceDetail
{
    using MediatR;

    public class GetApiResourceDetailQuery : IRequest<ApiResourceDetail>
    {
        public string Name { get; set; }
    }
}
