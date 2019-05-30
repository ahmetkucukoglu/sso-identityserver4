namespace SSO.Application.Client.Commands.UploadLogo
{
    using Domain.EntityFramework;
    using SSO.Infrastructure.Storage;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class UploadLogoCommandHandler : IRequestHandler<UploadLogoCommand, Unit>
    {
        private readonly AuthDbContext _authContext;
        private readonly IStorageService _storageService;

        public UploadLogoCommandHandler(AuthDbContext authContext, IStorageService storageService)
        {
            _authContext = authContext;
            _storageService = storageService;
        }

        public async Task<Unit> Handle(UploadLogoCommand request, CancellationToken cancellationToken)
        {
            var mediaLink = Upload(request);

            var client = await _authContext
                .Clients
                .FirstOrDefaultAsync((x) => x.Id == request.Id);

            client.LogoUri = mediaLink;

            await _authContext.SaveChangesAsync();

            return Unit.Value;
        }

        private string Upload(UploadLogoCommand request)
        {
            var memoryStream = new MemoryStream();
            request.LogoFile.OpenReadStream().CopyTo(memoryStream);

            var image = Image.FromStream(memoryStream);

            var scaleWidth = 130 / (float)image.Width;
            var scaleHeight = 180 / (float)image.Height;
            var scale = Math.Min(scaleHeight, scaleWidth);

            var newWidth = (int)(image.Width * scale);
            var newHeight = (int)(image.Height * scale);
        
            var imageEncoder = GetEncoder(ImageFormat.Png);

            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

            var ms = new MemoryStream();

            var bitmap = new Bitmap(image, newWidth, newHeight);
            bitmap.Save(ms, imageEncoder, encoderParameters);

            var mediaLink = _storageService.UploadFile(ms, request.Id + ".png", "LOGOS");
        
            return mediaLink;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();

            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }

            return null;
        }
    }
}
