namespace SSO.Application.User.Commands.CreateUser
{
    using MediatR;
    using System.Collections.Generic;

    public class CreateUserCommand : IRequest
    {
        public CreateUserCommand()
        {
            SelectedRoles = new List<string>();
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}
