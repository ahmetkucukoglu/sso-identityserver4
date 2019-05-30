namespace SSO.Application.ApiResource.Queries.GetApiResourceCreateViewModel
{
    using Commands.CreateApiResource;
    using MediatR;

    public class ApiResourceCreateViewModelQuery : IRequest<ApiResourceCreateViewModel>
    {
        public CreateApiResourceCommand Command { get; set; }
    }
}
