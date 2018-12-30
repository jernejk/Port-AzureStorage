using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Adapter.InMemory.Blobs
{
    public class Blob : IBlob
    {
        private readonly IBlobContainer _blobContainer;

        private readonly IBlobClient _blobClient;

        private readonly IBlobDirectory _blobDirectory;

        private readonly string _blobName;

        private byte[] _blobContent;

        private bool _exists;

        public Blob(string blobName, IBlobClient blobClient, IBlobContainer container, IBlobDirectory blobDirectory)
        {
            _blobName = blobName;
            _exists = false;
            _blobContainer = container;
            _blobClient = blobClient;
            _blobDirectory = blobDirectory;
        }

        protected Blob(string blobName)
        {
            _blobName = blobName;
            _exists = false;
        }

        public string Name
        {
            get
            {
                var name = _blobName;

                if (_blobDirectory != null)
                {
                    name = _blobDirectory.Name + "/" + name;
                }

                return name;
            }
        }

        public Uri Uri
        {
            get
            {
                return new Uri($"file://localblobstore/{_blobContainer.Name}/{Name}", UriKind.Absolute);
            }
        }

        public DateTime LastModifiedTime { get; private set; }

        public Task<bool> ExistsAsync()
        {
            return Task.FromResult(_exists);
        }

        public virtual async Task UploadTextAsync(string blobContent)
        {
            await EnsureBlobContainerExists();

            _blobContent = Encoding.UTF8.GetBytes(blobContent);
            _exists = true;
            LastModifiedTime = DateTime.UtcNow;
        }

        public async Task UploadFromStreamAsync(Stream stream)
        {
            await EnsureBlobContainerExists();

            if (stream != null)
            {
                _blobContent = new byte[stream.Length];
                stream.Read(_blobContent, 0, (int)stream.Length);

                LastModifiedTime = DateTime.UtcNow;
                _exists = true;
            }
        }

        public async Task<string> DownloadTextAsync()
        {
            await EnsureBlobContainerExists();

            await EnsureBlobExists();

            return Encoding.UTF8.GetString(_blobContent);
        }

        public async Task DownloadToStreamAsync(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            await EnsureBlobContainerExists();

            await EnsureBlobExists();

            stream.Write(_blobContent, 0, _blobContent.Length);
        }

        public async Task UploadFileAsync(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            await EnsureBlobContainerExists();

            _exists = true;
            LastModifiedTime = DateTime.UtcNow;
            _blobContent = File.ReadAllBytes(filePath);
        }

        public async Task DownloadToFileAsync(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            await EnsureBlobContainerExists();
            await EnsureBlobExists();

            File.WriteAllBytes(filePath, _blobContent);
        }

        private async Task EnsureBlobContainerExists()
        {
            if (!(await _blobClient.DoesBlobContainerExistAsync(_blobContainer.Name)))
            {
                throw new Exception();
            }
        }

        private async Task EnsureBlobExists()
        {
            if (!(await ExistsAsync()))
            {
                throw new Exception();
            }
        }
    }
}
