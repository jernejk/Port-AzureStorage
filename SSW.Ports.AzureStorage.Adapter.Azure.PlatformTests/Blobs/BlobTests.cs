using SSW.Ports.AzureStorage.Definition.Tests;

namespace SSW.Ports.AzureStorage.Adapter.Azure.PlatformTests.Blobs
{
    public class BlobTests : Definition.Tests.Blobs.BlobTests
    {
        public BlobTests()
        {
            BlobClient = StorageAccountFactory.Create().GetStorageAccount(PlatformTestConstants.PlatformTestStorageConnectionString).CreateBlobClient();
        }
    }
}
