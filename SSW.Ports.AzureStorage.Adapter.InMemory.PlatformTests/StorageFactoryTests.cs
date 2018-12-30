namespace SSW.Ports.AzureStorage.Adapter.InMemory.PlatformTests.Blob
{
    public class StorageFactoryTests : Definition.Tests.StorageFactoryTests
    {
        public StorageFactoryTests()
        {
            StorageFactory = new StorageAccountFactory();
        }
    }
}
