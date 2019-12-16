namespace SSO.Infrastructure.Storage
{
    using Amazon;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Microsoft.Extensions.Options;
    using System.IO;

    public class AwsStorage : IStorageService
    {
        private readonly IOptions<AwsStorageSettings> _options;

        public AwsStorage(IOptions<AwsStorageSettings> options)
        {
            _options = options;
        }

        public string UploadFile(MemoryStream memoryStream, string fileName, string directoryName)
        {
            var link = string.Empty;

            var fullPath = Path.Combine(directoryName, fileName).Replace("\\", "/");

            using (var s3Client = new AmazonS3Client(_options.Value.AccessKeyId, _options.Value.SecretAccessKey, RegionEndpoint.EUCentral1))
            {
                var request = new PutObjectRequest
                {
                    BucketName = _options.Value.BucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    InputStream = memoryStream,
                    Key = fullPath
                };

                var response = s3Client.PutObjectAsync(request).Result;

                link = $"https://{_options.Value.BucketName}.s3.eu-central-1.amazonaws.com/{fullPath}";
            }

            return link;
        }
    }
}
