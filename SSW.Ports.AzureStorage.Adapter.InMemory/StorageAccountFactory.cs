using System;
using System.Collections.Generic;
using SSW.Ports.AzureStorage.Definition;

namespace SSW.Ports.AzureStorage.Adapter.InMemory
{
    public class StorageAccountFactory : IStorageAccountFactory
    {
        private readonly Dictionary<string, IStorageAccount> _storageAccounts = new Dictionary<string, IStorageAccount>();

        public IStorageAccount GetStorageAccount(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (connectionString.Equals(AzureStorageConstants.InvalidConnectionString, StringComparison.OrdinalIgnoreCase))
            {
                throw new FormatException(nameof(connectionString));
            }

            if (!_storageAccounts.ContainsKey(connectionString))
            {
                _storageAccounts.Add(connectionString, new StorageAccount(connectionString));
            }

            return _storageAccounts[connectionString];
        }
    }
}
