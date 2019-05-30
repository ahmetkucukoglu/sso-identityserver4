namespace SSO.Infrastructure.Storage
{
    using Google.Apis.Auth.OAuth2;
    using Google.Cloud.Storage.V1;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.Options;
    using System.IO;

    public class GoogleCloudStorage : IStorageService
    {
        private readonly string _jsonFullPath;
        private readonly IOptions<GoogleCloudStorageSettings> _options;
        private readonly IContentTypeProvider _contentTypeProvider;

        public GoogleCloudStorage(IOptions<GoogleCloudStorageSettings> options, IHostingEnvironment hostingEnvironment, IContentTypeProvider contentTypeProvider)
        {
            _options = options;
            _contentTypeProvider = contentTypeProvider;

            _jsonFullPath = hostingEnvironment.ContentRootFileProvider.GetFileInfo(_options.Value.JsonFile).PhysicalPath;
        }

        public string UploadFile(MemoryStream memoryStream, string fileName, string directoryName)
        {
            var credential = GoogleCredential.FromFile(_jsonFullPath);
            var storageClient = StorageClient.Create(credential);

            var fullPath = Path.Combine(directoryName, fileName).Replace("\\", "/");
            _contentTypeProvider.TryGetContentType(fileName, out var contentType);

            var imageObject = storageClient.UploadObject(_options.Value.BucketName, fullPath, contentType, memoryStream,
                options: new UploadObjectOptions
                {
                    PredefinedAcl = PredefinedObjectAcl.PublicRead
                });

            return imageObject.MediaLink;
        }
    }
}
