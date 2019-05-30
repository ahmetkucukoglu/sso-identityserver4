namespace SSO.Application.Consent.Queries.GetConsentDetail
{
    using MediatR;

    public class GetConsentDetailQuery : IRequest<ConsentDetail>
    {
        public string ReturnUrl { get; set; }
    }
}
