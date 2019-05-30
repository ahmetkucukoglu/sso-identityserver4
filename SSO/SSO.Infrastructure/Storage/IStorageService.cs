namespace SSO.Infrastructure.Storage
{
    using System.IO;

    public interface IStorageService
    {
        string UploadFile(MemoryStream memoryStream, string fileName, string directoryName);
    }
}
