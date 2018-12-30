using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Adapter.Azure.Blobs
{
    public class BlobDirectory : IBlobDirectory
    {
        private readonly CloudBlobDirectory _cloudBlobDirectory;

        public BlobDirectory(CloudBlobDirectory cloudBlobDirectory, string directoryName)
        {
            if (cloudBlobDirectory == null)
            {
                throw new ArgumentNullException(nameof(cloudBlobDirectory));
            }

            if (string.IsNullOrEmpty(directoryName))
            {
                throw new ArgumentNullException(nameof(directoryName));
            }

            _cloudBlobDirectory = cloudBlobDirectory;
            Name = directoryName;
        }

        public string Name { get; private set; }

        public IBlob GetBlob(string blobName)
        {
            return new Blob(_cloudBlobDirectory.GetBlockBlobReference(blobName));
        }

        public async Task<IEnumerable<IBlob>> ListBlobsAsync(string blobNamePrefix, int maxResults, DateTime lastAccessedTime)
        {
            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;
            var blobs = Enumerable.Empty<IBlob>();

            do
            {
                resultSegment = await _cloudBlobDirectory.ListBlobsSegmentedAsync(true, BlobListingDetails.All, maxResults, continuationToken, null, null);
                blobs = blobs.Concat(resultSegment.Results.Select(i => new Blob(i as CloudBlockBlob)).Where(b => b != null));
                continuationToken = resultSegment.ContinuationToken;
            }
            while (continuationToken != null);

            return blobs;
        }
    }
}
