namespace SSO.Application.User.Queries.GetUserDetail
{
    using MediatR;

    public class GetUserDetailQuery : IRequest<UserDetail>
    {
        public string Id { get; set; }
    }
}
