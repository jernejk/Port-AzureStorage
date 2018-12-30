using System.Collections.Generic;
using System.Threading.Tasks;
using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Adapter.InMemory.Blobs
{
    public class BlobContainer : IBlobContainer
    {
        private readonly Dictionary<string, IBlob> _blobsList;

        private readonly Dictionary<string, IBlobDirectory> _blobsDirectoryList;

        private bool _isBlobPermissionsPublic;

        public BlobContainer()
        {
            Created = false;
            _blobsList = new Dictionary<string, IBlob>();
            _blobsDirectoryList = new Dictionary<string, IBlobDirectory>();
        }

        public IBlobClient BlobClient { get; set; }

        public string Name { get; set; }

        public bool Created { get; private set; }

        public Task CreateAsync()
        {
            Created = true;

            return Task.CompletedTask;
        }

        public IBlob GetBlob(string blobName)
        {
            if (!_blobsList.ContainsKey(blobName))
            {
                _blobsList.Add(blobName, CreateBlob(blobName));
            }

            return _blobsList[blobName];
        }

        public IBlobDirectory GetBlobDirectory(string directoryName)
        {
            if (!_blobsDirectoryList.ContainsKey(directoryName))
            {
                _blobsDirectoryList[directoryName] = new BlobDirectory(
                    directoryName, BlobClient, BlobClient.GetBlobContainer(Name));
            }

            return _blobsDirectoryList[directoryName];
        }

        public Task SetBlobPermissionsToPublicAsync()
        {
            _isBlobPermissionsPublic = true;

            return Task.CompletedTask;
        }

        public Task<bool> IsBlobPermissionsPublicAsync()
        {
            return Task.FromResult(_isBlobPermissionsPublic);
        }

        public Task DeleteIfExistsAsync()
        {
            _blobsDirectoryList.Clear();
            _blobsList.Clear();
            Created = false;

            return Task.CompletedTask;
        }

        protected virtual Blob CreateBlob(string blobName)
        {
            return new Blob(blobName, BlobClient, BlobClient.GetBlobContainer(Name), null);
        }
    }
}
