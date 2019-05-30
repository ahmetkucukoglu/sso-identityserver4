namespace SSO.Application.User.Commands.UpdateUser
{
    using MediatR;
    using System;
    using System.Collections.Generic;

    public class UpdateUserCommand : IRequest
    {
        public UpdateUserCommand()
        {
            SelectedRoles = new List<string>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string GivenName { get; set; }

        internal int Count()
        {
            throw new NotImplementedException();
        }

        public string FamilyName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}
