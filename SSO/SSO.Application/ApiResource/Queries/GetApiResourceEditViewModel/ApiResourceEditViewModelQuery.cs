namespace SSO.Application.ApiResource.Queries.GetApiResourceEditViewModel
{
    using Commands.UpdateApiResource;
    using MediatR;

    public class ApiResourceEditViewModelQuery : IRequest<ApiResourceEditViewModel>
    {
        public string Name { get; set; }
        public UpdateApiResourceCommand Command { get; set; }
    }
}
