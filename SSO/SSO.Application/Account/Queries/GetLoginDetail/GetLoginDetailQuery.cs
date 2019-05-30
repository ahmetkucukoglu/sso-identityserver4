namespace SSO.Application.Account.Queries.GetLoginDetail
{
    using MediatR;

    public class GetLoginDetailQuery : IRequest<LoginDetail>
    {
        public string ReturnUrl { get; set; }
    }
}
