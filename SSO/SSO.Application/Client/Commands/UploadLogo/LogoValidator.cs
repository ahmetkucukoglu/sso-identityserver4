namespace SSO.Application.Client.Commands.UploadLogo
{
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;
    using System.Linq;

    public class LogoValidator : AbstractValidator<IFormFile>
    {
        public LogoValidator()
        {
            RuleFor(x => x).NotEmpty().Must((x) =>
            {
                if (x != null)
                {
                    var fileFormats = new[] { ".jpeg", ".jpg", ".png" };
                    var fileExtension = Path.GetExtension(x.FileName);

                    var result = fileFormats.Contains(fileExtension);

                    return result;
                }

                return true;
            }).WithMessage("You must select .jpeg,.jpg,.png formats for logo");
        }
    }
}
