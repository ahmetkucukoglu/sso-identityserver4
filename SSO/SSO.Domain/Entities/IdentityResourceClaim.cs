namespace SSO.Domain.Entities
{
    public class IdentityResourceClaim
    {
        public string IdentityResourceId { get; set; }
        public IdentityResource IdentityResource { get; set; }
        public string ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}
