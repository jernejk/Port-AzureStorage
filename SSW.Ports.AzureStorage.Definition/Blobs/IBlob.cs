namespace SSW.Ports.AzureStorage.Definition.Blobs
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public interface IBlob
    {
        string Name { get; }

        Uri Uri { get; }

        DateTime LastModifiedTime { get; }

        Task UploadTextAsync(string blobContent);

        Task<string> DownloadTextAsync();

        Task<bool> ExistsAsync();

        Task UploadFromStreamAsync(Stream stream);

        Task DownloadToStreamAsync(Stream stream);

        Task UploadFileAsync(string filePath);

        Task DownloadToFileAsync(string filePath);
    }
}
