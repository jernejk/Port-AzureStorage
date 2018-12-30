using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Adapter.Azure.Blobs
{
    public class BlobContainer : IBlobContainer
    {
        private readonly CloudBlobContainer _cloudBlobContainer;

        public BlobContainer(CloudBlobContainer cloudBlobContainer, string containerName)
        {
            if (cloudBlobContainer == null)
            {
                throw new ArgumentNullException(nameof(cloudBlobContainer));
            }

            if (string.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentNullException(nameof(containerName));
            }

            _cloudBlobContainer = cloudBlobContainer;
            Name = containerName;
        }

        public string Name { get; private set; }

        public async Task CreateAsync()
        {
            await _cloudBlobContainer.CreateIfNotExistsAsync();
        }

        public IBlob GetBlob(string blobName)
        {
            return new Blob(_cloudBlobContainer.GetBlockBlobReference(blobName));
        }

        public IBlobDirectory GetBlobDirectory(string directoryName)
        {
            return new BlobDirectory(_cloudBlobContainer.GetDirectoryReference(directoryName), directoryName);
        }

        public async Task SetBlobPermissionsToPublicAsync()
        {
            var blobContainerPermissions = await _cloudBlobContainer.GetPermissionsAsync();
            blobContainerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            await _cloudBlobContainer.SetPermissionsAsync(blobContainerPermissions);
        }

        public async Task<bool> IsBlobPermissionsPublicAsync()
        {
            return (await _cloudBlobContainer.GetPermissionsAsync()).PublicAccess == BlobContainerPublicAccessType.Blob;
        }

        public async Task DeleteIfExistsAsync()
        {
            await _cloudBlobContainer.DeleteIfExistsAsync();
        }
    }
}
