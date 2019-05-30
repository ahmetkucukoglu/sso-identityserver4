namespace SSO.Application.ApiResource.Queries.GetApiResourceDetail
{
    using Commands.UpdateApiResource;
    using System.Collections.Generic;

    public class ApiResourceDetail
    {
        public ApiResourceDetail()
        {
            SelectedClaims = new List<string>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public IEnumerable<string> SelectedClaims { get; set; }

        public static explicit operator UpdateApiResourceCommand(ApiResourceDetail apiResourceDetail)
        {
            return new UpdateApiResourceCommand
            {
                Description = apiResourceDetail.Description,
                DisplayName = apiResourceDetail.DisplayName,
                Enabled = apiResourceDetail.Enabled,
                Name = apiResourceDetail.Name,
                SelectedClaims = apiResourceDetail.SelectedClaims
            };
        }
    }
}
