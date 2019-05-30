namespace SSO.Domain.Entities
{
    using System.Collections.Generic;

    public class ApiResource
    {
        public ApiResource()
        {
            Claims = new List<ApiResourceClaim>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public ICollection<ApiResourceClaim> Claims { get; set; }
    }
}
