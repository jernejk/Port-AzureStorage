using SSW.Ports.AzureStorage.Definition.Tests;

namespace SSW.Ports.AzureStorage.Adapter.Azure.PlatformTests
{
    public class StorageAccountTests : Definition.Tests.StorageAccountTests
    {
        public StorageAccountTests()
        {
            StorageAccount = (StorageAccount)StorageAccountFactory.Create().GetStorageAccount(PlatformTestConstants.PlatformTestStorageConnectionString);
        }
    }
}
