namespace SSO.Application.ApiResource.Commands.DeleteApiResource
{
    using MediatR;

    public class DeleteApiResourceCommand : IRequest
    {
        public string Name { get; set; }
    }
}
