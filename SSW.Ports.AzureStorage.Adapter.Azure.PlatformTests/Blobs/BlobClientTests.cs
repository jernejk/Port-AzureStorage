using SSW.Ports.AzureStorage.Definition.Tests;

namespace SSW.Ports.AzureStorage.Adapter.Azure.PlatformTests.Blobs
{
    public class BlobClientTests : Definition.Tests.Blobs.BlobClientTests
    {
        public BlobClientTests()
        {
            BlobClient = StorageAccountFactory.Create().GetStorageAccount(PlatformTestConstants.PlatformTestStorageConnectionString).CreateBlobClient();
            BlobClientBaseAddress = PlatformTestConstants.PlatformTestBlobClientBaseAddress;
        }
    }
}
