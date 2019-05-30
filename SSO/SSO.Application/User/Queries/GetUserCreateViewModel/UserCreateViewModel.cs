namespace SSO.Application.User.Queries.GetUserCreateViewModel
{
    using Commands.CreateUser;
    using System.Collections.Generic;

    public class UserCreateViewModel
    {
        public UserCreateViewModel()
        {
            Command = new CreateUserCommand();
            Roles = new List<string>();
        }

        public CreateUserCommand Command { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
