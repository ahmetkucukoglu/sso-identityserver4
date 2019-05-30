namespace SSO.Application.ApiResource.Commands.UpdateApiResource
{
    using MediatR;
    using System.Collections.Generic;

    public class UpdateApiResourceCommand : IRequest
    {
        public UpdateApiResourceCommand()
        {
            SelectedClaims = new List<string>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public IEnumerable<string> SelectedClaims { get; set; }
    }
}
