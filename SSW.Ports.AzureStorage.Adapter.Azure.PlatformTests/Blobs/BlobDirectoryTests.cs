using SSW.Ports.AzureStorage.Definition.Tests;

namespace SSW.Ports.AzureStorage.Adapter.Azure.PlatformTests.Blobs
{
    public class BlobDirectoryTests : Definition.Tests.Blobs.BlobDirectoryTests
    {
        public BlobDirectoryTests()
        {
            BlobContainer = StorageAccountFactory.Create()
                .GetStorageAccount(PlatformTestConstants.PlatformTestStorageConnectionString)
                .CreateBlobClient()
                .GetBlobContainer("blobcontainertestdirectory");
        }
    }
}
