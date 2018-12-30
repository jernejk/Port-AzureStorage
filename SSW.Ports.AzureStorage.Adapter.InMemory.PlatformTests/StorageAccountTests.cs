namespace SSW.Ports.AzureStorage.Adapter.InMemory.PlatformTests
{
    public class StorageAccountTests : Definition.Tests.StorageAccountTests
    {
        public StorageAccountTests()
        {
            StorageAccount = new StorageAccount("TestAccount");
        }
    }
}
