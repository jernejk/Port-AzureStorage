using System.Globalization;

namespace SSW.Ports.AzureStorage.Definition.Tests
{
    public static class PlatformTestConstants
    {
        public const string PlatformTestStorageConnectionStringForEmulator = "UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://127.0.0.1";

        public static readonly string PlatformTestBlobClientBaseAddress = string.Format(
            CultureInfo.InvariantCulture, "https://{0}.blob.core.windows.net/", StorageAccountName);

        public static readonly string PlatformTestStorageConnectionString = string.Format(
            CultureInfo.InvariantCulture,
            "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
            StorageAccountName,
            StorageAccountKey);

        private const string StorageAccountName = "knplatformtests";

        private const string StorageAccountKey = "PKANiZAEhUKhi/pfeF0AFp5fx+FlPxOB62DHY3NlsSbiemkW9WvUNEiikyCfTvrhFdqUMDS6IlfoLDUrRNstoQ==";
    }
}
