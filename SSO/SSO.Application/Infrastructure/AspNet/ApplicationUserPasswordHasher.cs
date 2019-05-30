namespace SSO.Application.Infrastructure.AspNet
{
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class ApplicationUserPasswordHasher : IPasswordHasher<ApplicationUser>
    {
        public string HashPassword(ApplicationUser user, string password)
        {
            var byteData = Encoding.Unicode.GetBytes(password);
            var byteEncryptSha = new SHA1CryptoServiceProvider().ComputeHash(byteData);

            return Convert.ToBase64String(byteEncryptSha);
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == HashPassword(user, providedPassword))
                return PasswordVerificationResult.Success;

            return PasswordVerificationResult.Failed;
        }
    }
}
