using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Definition
{
    public interface IStorageAccount
    {
        string ConnectionString { get; }

        IBlobClient CreateBlobClient();
    }
}
