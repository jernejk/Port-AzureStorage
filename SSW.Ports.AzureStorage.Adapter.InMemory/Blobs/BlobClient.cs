using System.Collections.Generic;
using System.Threading.Tasks;
using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Adapter.InMemory.Blobs
{
    public class BlobClient : IBlobClient
    {
        public BlobClient()
        {
            ContainersList = new Dictionary<string, BlobContainer>();
        }

        protected Dictionary<string, BlobContainer> ContainersList { get; private set; }

        public IBlobContainer GetBlobContainer(string containerName)
        {
            if (!ContainersList.ContainsKey(containerName))
            {
                var blobContainer = CreateBlobContainer(containerName);
                blobContainer.Name = containerName;
                blobContainer.BlobClient = this;

                ContainersList.Add(containerName, blobContainer);
            }

            return ContainersList[containerName];
        }

        public Task<bool> DoesBlobContainerExistAsync(string containerName)
        {
            var res = ContainersList != null && ContainersList.ContainsKey(containerName)
                   && ContainersList[containerName].Created;

            return Task.FromResult(res);
        }

        public string GetBaseAddress()
        {
            return Constants.DevelopmentStorageBlobClientEndpoint;
        }

        protected virtual BlobContainer CreateBlobContainer(string containerName)
        {
            return new BlobContainer();
        }
    }
}
