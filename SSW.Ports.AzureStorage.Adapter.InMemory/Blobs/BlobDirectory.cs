using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Adapter.InMemory.Blobs
{
    public class BlobDirectory : IBlobDirectory
    {
        private readonly IBlobContainer _container;

        private readonly IBlobClient _blobClient;

        private readonly Dictionary<string, IBlob> _blobsList = new Dictionary<string, IBlob>();

        public BlobDirectory(string name)
        {
            Name = name;
        }

        public BlobDirectory(string name, IBlobClient blobClient, IBlobContainer container)
        {
            Name = name;
            _container = container;
            _blobClient = blobClient;
        }

        public string Name { get; private set; }

        public IBlob GetBlob(string blobName)
        {
            if (!_blobsList.ContainsKey(blobName))
            {
                _blobsList.Add(blobName, new Blob(blobName, _blobClient, _container, this));
            }

            return _blobsList[blobName];
        }

        public Task<IEnumerable<IBlob>> ListBlobsAsync(string blobNamePrefix, int pageSize, DateTime lastAccessedTime)
        {
            var candidateList =
                _blobsList.Where(
                    x => x.Value.Name.StartsWith(blobNamePrefix, StringComparison.OrdinalIgnoreCase) && x.Value.LastModifiedTime > lastAccessedTime);

            return Task.FromResult(candidateList.Select(candidate => candidate.Value));
        }
    }
}
