using System;
using SSW.Ports.AzureStorage.Adapter.InMemory.Blobs;
using SSW.Ports.AzureStorage.Definition;
using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Adapter.InMemory
{
    public class StorageAccount : IStorageAccount
    {
        public const string UsableTestConnectionString = "UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://127.0.0.1";

        private readonly BlobClient _blobClient;

        public StorageAccount()
        {
            _blobClient = new BlobClient();
        }

        public StorageAccount(string connectionString)
            : this()
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            ConnectionString = connectionString;
        }

        public string ConnectionString { get; private set; }

        public IBlobClient CreateBlobClient()
        {
            return _blobClient;
        }
    }
}
