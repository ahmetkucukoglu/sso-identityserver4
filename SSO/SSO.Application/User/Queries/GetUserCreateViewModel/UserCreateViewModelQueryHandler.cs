namespace SSO.Application.User.Queries.GetUserCreateViewModel
{
    using Commands.CreateUser;
    using MediatR;
    using Role.Queries.GetRoleList;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class UserCreateViewModelQueryHandler : IRequestHandler<UserCreateViewModelQuery, UserCreateViewModel>
    {
        private readonly IMediator _mediator;

        public UserCreateViewModelQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<UserCreateViewModel> Handle(UserCreateViewModelQuery request, CancellationToken cancellationToken)
        {
            var viewModel = new UserCreateViewModel
            {
                Command = request.Command ?? new CreateUserCommand(),
                Roles = (await _mediator.Send(new GetRoleListQuery())).Select((x) => x.Name)
            };

            return viewModel;
        }
    }
}
