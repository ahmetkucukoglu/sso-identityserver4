namespace SSO.Application.ApiResource.Queries.GetApiResourceEditViewModel
{
    using Commands.UpdateApiResource;
    using System.Collections.Generic;

    public class ApiResourceEditViewModel
    {
        public ApiResourceEditViewModel()
        {
            Command = new UpdateApiResourceCommand();
            Claims = new List<string>();
        }

        public IEnumerable<string> Claims { get; set; }
        public UpdateApiResourceCommand Command { get; set; }
    }
}
