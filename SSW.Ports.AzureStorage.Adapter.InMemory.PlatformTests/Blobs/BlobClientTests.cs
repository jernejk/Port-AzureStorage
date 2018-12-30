namespace SSW.Ports.AzureStorage.Adapter.InMemory.PlatformTests.Blobs
{
    public class BlobClientTests : Definition.Tests.Blobs.BlobClientTests
    {
        public BlobClientTests()
        {
            BlobClient =
                new StorageAccountFactory().GetStorageAccount("UseInMemoryDevelopmentStorage=true")
                    .CreateBlobClient();
            BlobClientBaseAddress = Constants.DevelopmentStorageBlobClientEndpoint;
        }
    }
}
