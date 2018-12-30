namespace SSW.Ports.AzureStorage.Adapter.InMemory.PlatformTests.Blobs
{
    public class BlobDirectoryTests : Definition.Tests.Blobs.BlobDirectoryTests
    {
        public BlobDirectoryTests()
        {
            BlobContainer =
                new StorageAccountFactory().GetStorageAccount("UseInMemoryDevelopmentStorage=true")
                    .CreateBlobClient().GetBlobContainer("BlobContainerTestDirectory");
        }
    }
}
