namespace SSO.Application.Infrastructure.IdentityServer
{
    using Application.Account.Commands.Login;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using User.Commands.ChangePassword;
    using User.Commands.CreateUser;
    using User.Commands.DeleteUser;
    using User.Commands.ForgotPassword;
    using User.Commands.ResetPassword;
    using User.Commands.UpdateUser;
    using User.Queries.GetUserDetail;
    using User.Queries.GetUserList;

    public interface IIdentityUserService
    {
        Task ChangePassword(ChangePasswordCommand request);
        Task CreateUser(CreateUserCommand request);
        Task DeleteUser(DeleteUserCommand request);
        Task<bool> ForgotPassword(ForgotPasswordCommand request);
        Task ResetPassword(ResetPasswordCommand request);
        Task UpdateUser(UpdateUserCommand request);
        Task<UserDetail> GetUserDetail(GetUserDetailQuery request);
        Task<IEnumerable<UserListItem>> GetUserList(GetUserListQuery request);
        Task<string> Login(LoginCommand request);
    }
}
