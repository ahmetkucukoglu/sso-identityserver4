namespace SSO.Application.Error.Queries.GetErrorDetail
{
    using MediatR;

    public class GetErrorDetailQuery : IRequest<ErrorDetail>
    {
        public string Id { get; set; }
    }
}
