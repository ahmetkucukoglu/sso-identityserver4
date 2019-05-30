namespace SSO.Application.User.Queries.GetUserEditViewModel
{
    using Commands.UpdateUser;
    using MediatR;

    public class UserEditViewModelQuery : IRequest<UserEditViewModel>
    {
        public string Id { get; set; }
        public UpdateUserCommand Command { get; set; }
    }
}
