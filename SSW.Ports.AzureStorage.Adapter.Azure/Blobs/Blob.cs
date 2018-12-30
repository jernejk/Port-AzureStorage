using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Adapter.Azure.Blobs
{
    public class Blob : IBlob
    {
        private readonly CloudBlockBlob _cloudBlockBlob;

        public Blob(CloudBlockBlob cloudBlockBlob)
        {
            if (cloudBlockBlob == null)
            {
                throw new ArgumentNullException(nameof(cloudBlockBlob));
            }

            _cloudBlockBlob = cloudBlockBlob;
        }

        public string Name => _cloudBlockBlob.Name;

        public Uri Uri => _cloudBlockBlob.Uri;

        public DateTime LastModifiedTime
        {
            get
            {
                var lastModified = _cloudBlockBlob.Properties.LastModified;
                if (lastModified.HasValue)
                {
                    return ((DateTimeOffset)lastModified).DateTime;
                }

                throw new InvalidOperationException();
            }
        }

        public async Task UploadTextAsync(string blobContent)
        {
            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(blobContent)))
            {
                await _cloudBlockBlob.UploadFromStreamAsync(memoryStream);
            }
        }

        public async Task<string> DownloadTextAsync()
        {
            using (var memoryStream = new MemoryStream())
            {
                await _cloudBlockBlob.DownloadToStreamAsync(memoryStream);
                var reader = new StreamReader(memoryStream);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                return reader.ReadToEnd();
            }
        }

        public Task<bool> ExistsAsync()
        {
            return _cloudBlockBlob.ExistsAsync();
        }

        public Task UploadFromStreamAsync(Stream stream)
        {
            return _cloudBlockBlob.UploadFromStreamAsync(stream);
        }

        public Task DownloadToStreamAsync(Stream stream)
        {
            return _cloudBlockBlob.DownloadToStreamAsync(stream);
        }

        public async Task UploadFileAsync(string filePath)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    memoryStream.SetLength(fileStream.Length);
                    fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
                    await _cloudBlockBlob.UploadFromStreamAsync(memoryStream);
                }
            }
        }

        public async Task DownloadToFileAsync(string filePath)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    await _cloudBlockBlob.DownloadToStreamAsync(memoryStream);
                    var bytes = new byte[memoryStream.Length];
                    memoryStream.Position = 0;
                    memoryStream.Read(bytes, 0, (int)memoryStream.Length);
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }
}
