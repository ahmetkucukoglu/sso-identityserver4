namespace SSO.Application.User.Queries.GetUserEditViewModel
{
    using Commands.UpdateUser;
    using System.Collections.Generic;

    public class UserEditViewModel
    {
        public UserEditViewModel()
        {
            Command = new UpdateUserCommand();
            Roles = new List<string>();
        }

        public UpdateUserCommand Command { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
