using System.Threading.Tasks;

namespace SSW.Ports.AzureStorage.Definition.Blobs
{
    public interface IBlobContainer
    {
        string Name { get; }

        Task CreateAsync();

        IBlob GetBlob(string blobName);

        IBlobDirectory GetBlobDirectory(string directoryName);

        Task SetBlobPermissionsToPublicAsync();

        Task<bool> IsBlobPermissionsPublicAsync();

        Task DeleteIfExistsAsync();
    }
}
