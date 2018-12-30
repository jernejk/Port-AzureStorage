namespace SSW.Ports.AzureStorage.Definition
{
    public interface IStorageAccountFactory
    {
        IStorageAccount GetStorageAccount(string connectionString);
    }
}