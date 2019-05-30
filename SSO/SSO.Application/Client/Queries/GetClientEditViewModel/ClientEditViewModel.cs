namespace SSO.Application.Client.Queries.GetClientEditViewModel
{
    using Commands.UpdateClient;
    using System.Collections.Generic;

    public class ClientEditViewModel
    {
        public ClientEditViewModel()
        {
            Command = new UpdateClientCommand();
        }

        public UpdateClientCommand Command { get; set; }
        public IEnumerable<string> ApiResources { get; set; }
        public IEnumerable<string> IdentityResources { get; set; }
    }
}
