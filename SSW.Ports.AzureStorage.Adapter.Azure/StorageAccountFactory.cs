using SSW.Ports.AzureStorage.Definition;

namespace SSW.Ports.AzureStorage.Adapter.Azure
{
    public class StorageAccountFactory : IStorageAccountFactory
    {
        private StorageAccountFactory()
        {
        }

        public static StorageAccountFactory Create()
        {
            return new StorageAccountFactory();
        }

        public IStorageAccount GetStorageAccount(string connectionString)
        {
            return new StorageAccount(connectionString);
        }
    }
}
