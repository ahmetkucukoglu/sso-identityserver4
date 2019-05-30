namespace SSO.Infrastructure.Email
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task<bool> SendResetPasswordLink(string to, string name, string link);
    }
}
