using FluentAssertions;
using Xunit;

namespace SSW.Ports.AzureStorage.Definition.Tests
{
    public abstract class StorageAccountTests
    {
        protected IStorageAccount StorageAccount { get; set; }

        [Fact]
        public void StorageAccountCanCreateBlobClient()
        {
            var blobClient = StorageAccount.CreateBlobClient();

            blobClient.Should().NotBeNull();
        }
    }
}
