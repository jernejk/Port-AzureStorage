using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Adapter.Azure.Blobs
{
    public class BlobClient : IBlobClient
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public BlobClient(CloudStorageAccount cloudStorageAccount)
            : this(cloudStorageAccount, RetryPolicyFactory.CreateExponentialRetryPolicy())
        {
        }

        protected BlobClient(CloudStorageAccount cloudStorageAccount, IRetryPolicy retryPolicy)
        {
            if (cloudStorageAccount == null)
            {
                throw new ArgumentNullException(nameof(cloudStorageAccount));
            }

            if (retryPolicy == null)
            {
                throw new ArgumentNullException(nameof(retryPolicy));
            }

            _cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobClient.DefaultRequestOptions.RetryPolicy = retryPolicy;
        }

        public IBlobContainer GetBlobContainer(string containerName)
        {
            return new BlobContainer(_cloudBlobClient.GetContainerReference(containerName), containerName);
        }

        public async Task<bool> DoesBlobContainerExistAsync(string containerName)
        {
            return await _cloudBlobClient.GetContainerReference(containerName).ExistsAsync();
        }

        public string GetBaseAddress()
        {
            return _cloudBlobClient.BaseUri.AbsoluteUri;
        }
    }
}
