namespace SSW.Ports.AzureStorage.Adapter.InMemory.PlatformTests.Blobs
{
    public class BlobTests : Definition.Tests.Blobs.BlobTests
    {
        public BlobTests()
        {
            BlobClient =
                new StorageAccountFactory().GetStorageAccount("UseInMemoryDevelopmentStorage=true")
                    .CreateBlobClient();
        }
    }
}
