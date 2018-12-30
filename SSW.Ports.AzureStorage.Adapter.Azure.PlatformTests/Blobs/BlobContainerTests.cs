using SSW.Ports.AzureStorage.Definition.Tests;

namespace SSW.Ports.AzureStorage.Adapter.Azure.PlatformTests.Blobs
{
    public class BlobContainerTests : Definition.Tests.Blobs.BlobContainerTests
    {
        public BlobContainerTests()
        {
            BlobClient =
                StorageAccountFactory.Create()
                                     .GetStorageAccount(PlatformTestConstants.PlatformTestStorageConnectionString)
                                     .CreateBlobClient();
        }
    }
}
