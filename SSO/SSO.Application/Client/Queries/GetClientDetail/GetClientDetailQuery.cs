namespace SSO.Application.Client.Queries.GetClientDetail
{
    using MediatR;

    public class GetClientDetailQuery : IRequest<ClientDetail>
    {
        public string Id { get; set; }
    }
}
