using System;
using Microsoft.WindowsAzure.Storage;
using SSW.Ports.AzureStorage.Adapter.Azure.Blobs;
using SSW.Ports.AzureStorage.Definition;
using SSW.Ports.AzureStorage.Definition.Blobs;

namespace SSW.Ports.AzureStorage.Adapter.Azure
{
    public class StorageAccount : IStorageAccount
    {
        private readonly CloudStorageAccount _cloudStorageAccount;

        public StorageAccount(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            ConnectionString = connectionString;
            _cloudStorageAccount = CloudStorageAccount.Parse(ConnectionString);
        }

        public string ConnectionString { get; private set; }

        public IBlobClient CreateBlobClient()
        {
            return new BlobClient(_cloudStorageAccount);
        }
    }
}
