namespace SSO.Domain.Entities
{
    public class ApiResourceClaim
    {
        public string ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
        public string ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}
