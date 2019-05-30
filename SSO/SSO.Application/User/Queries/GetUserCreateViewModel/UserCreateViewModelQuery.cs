namespace SSO.Application.User.Queries.GetUserCreateViewModel
{
    using Commands.CreateUser;
    using MediatR;

    public class UserCreateViewModelQuery : IRequest<UserCreateViewModel>
    {
        public CreateUserCommand Command { get; set; }
    }
}
