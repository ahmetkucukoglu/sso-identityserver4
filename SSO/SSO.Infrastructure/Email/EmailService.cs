namespace SSO.Infrastructure.Email
{
    using Microsoft.Extensions.Configuration;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Net;
    using System.Threading.Tasks;

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendResetPasswordLink(string toEmailAddress, string name, string link)
        {
            var apiKey = _configuration.GetSection("SendGrid:ApiKey").Value;
            var resetPasswordTemplateId = _configuration.GetSection("SendGrid:ResetPasswordTemplateId").Value;

            var sendGridClient = new SendGridClient(apiKey);
            var from = new EmailAddress("info@yourdomain.com", "SSO");
            var to = new EmailAddress(toEmailAddress, name);

            var sendGridMessage = MailHelper.CreateSingleTemplateEmail(from, to, resetPasswordTemplateId, new { link });

            var response = await sendGridClient.SendEmailAsync(sendGridMessage);

            return response.StatusCode == HttpStatusCode.Accepted;
        }
    }
}
