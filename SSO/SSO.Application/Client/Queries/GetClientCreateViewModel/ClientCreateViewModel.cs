namespace SSO.Application.Client.Queries.GetClientCreateViewModel
{
    using Commands.CreateClient;
    using System.Collections.Generic;

    public class ClientCreateViewModel
    {
        public ClientCreateViewModel()
        {
            Command = new CreateClientCommand();
        }

        public CreateClientCommand Command { get; set; }
        public IEnumerable<string> ApiResources { get; set; }
        public IEnumerable<string> IdentityResources { get; set; }
    }
}
