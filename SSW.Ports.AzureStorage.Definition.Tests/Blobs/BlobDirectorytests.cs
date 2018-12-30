using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SSW.Ports.AzureStorage.Definition.Blobs;
using Xunit;

namespace SSW.Ports.AzureStorage.Definition.Tests.Blobs
{
    public abstract class BlobDirectoryTests
    {
        protected IBlobContainer BlobContainer { get; set; }

        [Fact]
        public void GetBlobDirectoryFromBlobContainerWithAGivenNameShouldGiveABlobDirectoryWithThatName()
        {
            var directoryName = AzureResourceUniqueNameCreator.CreateUniqueBlobDirectoryName();

            var blobDirectory = BlobContainer.GetBlobDirectory(directoryName);

            blobDirectory.Name.Should().Be(directoryName);
        }

        [Fact]
        public void GetBlobShouldReturnInstanceOfBlobFromBlobDirectoryEvenIfBlobDirectoryAndBlobDoesNotExists()
        {
            var directoryName = AzureResourceUniqueNameCreator.CreateUniqueBlobDirectoryName();
            var blobName = AzureResourceUniqueNameCreator.CreateUniqueBlobName();

            var blob = GetBlobDirectoryWithName(directoryName).GetBlob(blobName);

            blob.Name.Should().Be(directoryName + "/" + blobName);
        }

        [Fact]
        public async Task UploadTextToABlobInBlobDirectoryUploadsTheText()
        {
            var directoryName = AzureResourceUniqueNameCreator.CreateUniqueBlobDirectoryName();
            await BlobContainer.CreateAsync();

            const string ContentToUpload = "Blob content";
            var blobName = AzureResourceUniqueNameCreator.CreateUniqueBlobName();
            await GetBlobDirectoryWithName(directoryName).GetBlob(blobName).UploadTextAsync(ContentToUpload);

            var blobContent = await GetBlobDirectoryWithName(directoryName).GetBlob(blobName).DownloadTextAsync();
            blobContent.Should().Contain(ContentToUpload);
        }

        [Fact]
        public async Task ListBlobShouldReturnAllBlobsInDirectoryWithFullBlobName()
        {
            var directoryName = AzureResourceUniqueNameCreator.CreateUniqueBlobDirectoryName();
            await BlobContainer.CreateAsync();

            var contentOfBlobs = new[]
                {
                    await UploadABlobInTheGivenDirectoryAndReturnItsName(directoryName),
                    await UploadABlobInTheGivenDirectoryAndReturnItsName(directoryName)
                };

            var blobsFromDirectory = await GetBlobDirectoryWithName(directoryName).ListBlobsAsync(string.Empty, contentOfBlobs.Length, DateTime.MinValue);

            blobsFromDirectory.Select(blob => blob.Name).Should().Contain(contentOfBlobs);
        }

        private IBlobDirectory GetBlobDirectoryWithName(string directoryName)
        {
            return BlobContainer.GetBlobDirectory(directoryName);
        }

        private async Task<string> UploadABlobInTheGivenDirectoryAndReturnItsName(string directoryName)
        {
            var blobName = AzureResourceUniqueNameCreator.CreateUniqueBlobName();

            var blob = GetBlobDirectoryWithName(directoryName).GetBlob(blobName);

            var textToUpload = string.Format(CultureInfo.InvariantCulture, "Content for {0}", blobName);
            await blob.UploadTextAsync(textToUpload);

            return directoryName + "/" + blobName;
        }
    }
}
