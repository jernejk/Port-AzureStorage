namespace SSW.Ports.AzureStorage.Adapter.Azure.PlatformTests
{
    public class StorageFactoryTests : Definition.Tests.StorageFactoryTests
    {
        public StorageFactoryTests()
        {
            StorageFactory = StorageAccountFactory.Create();
        }
    }
}
