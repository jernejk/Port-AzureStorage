using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SSW.Ports.AzureStorage.Definition.Tests
{
    public abstract class StorageFactoryTests
    {
        protected IStorageAccountFactory StorageFactory { get; set; }

        [Fact]
        public void PlatformCanCreateStorageAccountFromStorageFactory()
        {
            StorageFactory.GetStorageAccount(PlatformTestConstants.PlatformTestStorageConnectionStringForEmulator).Should().NotBeNull();
        }

        [Fact]
        public void PlatformShouldThrowExceptionIfConnectionStringIsNotValid()
        {
            Action action = () => StorageFactory.GetStorageAccount("ThisIsAnInvalidConnectionString");

            action.Should().Throw<FormatException>();
        }

        [Fact]
        public void PlatformShouldRetainConnectionStringInTheCreatedStorageAccount()
        {
            var connectionString = AzureResourceUniqueNameCreator.CreateUniqueConnectionString();
            StorageFactory.GetStorageAccount(connectionString)
                .ConnectionString.Should()
                .Be(connectionString);
        }

        [Fact]
        public async Task PlatformCanCreateSameStorageAccountEvenIfRequestedMultipleTimes()
        {
            var connectionString = PlatformTestConstants.PlatformTestStorageConnectionString;
            const string ContainerName = "platformtest";
            const string BlobName = "platformtestblob";
            const string ExpectedValue = "This is platform test data for getting same storage";

            var storageAccount = StorageFactory.GetStorageAccount(connectionString);
            var container = storageAccount.CreateBlobClient().GetBlobContainer(ContainerName);
            await container.CreateAsync();
            await container.GetBlob(BlobName).UploadTextAsync(ExpectedValue);

            var valueInBlob = await StorageFactory.GetStorageAccount(connectionString)
                .CreateBlobClient()
                    .GetBlobContainer(ContainerName)
                        .GetBlob(BlobName)
                            .DownloadTextAsync();
            valueInBlob.Should().Be(ExpectedValue);
        }
    }
}
