namespace SSO.Application.User.Queries.GetUserEditViewModel
{
    using Commands.UpdateUser;
    using GetUserDetail;
    using MediatR;
    using Role.Queries.GetRoleList;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class UserEditViewModelQueryHandler : IRequestHandler<UserEditViewModelQuery, UserEditViewModel>
    {
        private readonly IMediator _mediator;

        public UserEditViewModelQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<UserEditViewModel> Handle(UserEditViewModelQuery request, CancellationToken cancellationToken)
        {
            var viewModel = new UserEditViewModel
            {
                Roles = (await _mediator.Send(new GetRoleListQuery())).Select((x) => x.Name)
            };

            if (!string.IsNullOrEmpty(request.Id))
            {
                var user = await _mediator.Send(new GetUserDetailQuery { Id = request.Id });

                var command = new UpdateUserCommand
                {
                    Email = user.Email,
                    EmailVerified = user.EmailVerified,
                    FamilyName = user.FamilyName,
                    GivenName = user.GivenName,
                    Id = user.Id,
                    SelectedRoles = user.SelectedRoles,
                    UserName = user.UserName
                };

                viewModel.Command = command;
            }
            else
            {
                viewModel.Command = request.Command;
            }

            return viewModel;
        }
    }
}
