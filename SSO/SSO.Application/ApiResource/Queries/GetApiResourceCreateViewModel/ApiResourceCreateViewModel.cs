namespace SSO.Application.ApiResource.Queries.GetApiResourceCreateViewModel
{
    using Commands.CreateApiResource;
    using System.Collections.Generic;

    public class ApiResourceCreateViewModel
    {
        public ApiResourceCreateViewModel()
        {
            Command = new CreateApiResourceCommand();
            Claims = new List<string>();
        }

        public IEnumerable<string> Claims { get; set; }
        public CreateApiResourceCommand Command { get; set; }
    }
}
