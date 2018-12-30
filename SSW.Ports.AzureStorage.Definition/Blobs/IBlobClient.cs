using System.Threading.Tasks;

namespace SSW.Ports.AzureStorage.Definition.Blobs
{
    public interface IBlobClient
    {
        IBlobContainer GetBlobContainer(string containerName);

        Task<bool> DoesBlobContainerExistAsync(string containerName);

        string GetBaseAddress();
    }
}
