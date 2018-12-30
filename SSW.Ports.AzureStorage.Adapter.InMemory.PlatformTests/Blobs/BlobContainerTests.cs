namespace SSW.Ports.AzureStorage.Adapter.InMemory.PlatformTests.Blobs
{
    public class BlobContainerTests : Definition.Tests.Blobs.BlobContainerTests
    {
        public BlobContainerTests()
        {
            BlobClient =
                new StorageAccountFactory().GetStorageAccount("UseInMemoryDevelopmentStorage=true")
                    .CreateBlobClient();
        }
    }
}
