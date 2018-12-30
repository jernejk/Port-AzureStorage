using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using SSW.Ports.AzureStorage.Definition.Blobs;
using Xunit;

namespace SSW.Ports.AzureStorage.Definition.Tests.Blobs
{
    public abstract class BlobTests : IDisposable
    {
        private const string SampleBlobContent = "Content to upload to blob";

        private const string SampleBlobName = "blob.txt";

        private readonly IList<Func<Task>> _cleanupTasks = new List<Func<Task>>();

        private bool _disposed = false;

        protected IBlobClient BlobClient { get; set; }

        [Fact]
        public void VerifyNameAndUri()
        {
            var c = GetBlobContainer();
            var d = GetBlobDirectory(c);
            var blob = GetBlob(d);

            blob.Uri.ToString().Should().EndWith($"{c.Name}/{blob.Name}");
        }

        [Fact]
        public void UploadFileShouldThrowExceptionIfBlobContainerDoesNotExist()
        {
            var blobContainer = GetBlobContainer();

            var blobDirectory = GetBlobDirectory(blobContainer);

            var blob = GetBlob(blobDirectory);

            var fileName = Path.GetTempFileName();

            Func<Task> action = async () => await blob.UploadFileAsync(fileName);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public async Task UploadFileShouldNotThrowExceptionIfBlobContainerExists()
        {
            var blob = await GetABlobFromAValidBlobContainer();

            var fileName = Path.GetTempFileName();

            Func<Task> action = async () => await blob.UploadFileAsync(fileName);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public async Task UploadFileShouldUploadTheFileIfBlobContainerExists()
        {
            var blob = await GetABlobFromAValidBlobContainer();

            var fileName = Path.GetTempFileName();

            const string Filecontent = "FileContent";
            File.AppendAllText(fileName, Filecontent);

            await blob.UploadFileAsync(fileName);

            var uploadedContent = await blob.DownloadTextAsync();

            uploadedContent.Should().Be(Filecontent);
        }

        [Fact]
        public async Task UploadFileShouldThrowIfFilePathDoesNotExist()
        {
            var blob = await GetABlobFromAValidBlobContainer();

            var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Func<Task> action = async () => await blob.UploadFileAsync(fileName);

            action.Should().Throw<FileNotFoundException>();
        }

        [Fact]
        public void DownloadToFileShouldThrowExceptionIfBlobContainerDoesNotExist()
        {
            var blobContainer = GetBlobContainer();

            var blobDirectory = GetBlobDirectory(blobContainer);

            var blob = GetBlob(blobDirectory);

            var fileName = Path.GetTempFileName();

            Func<Task> action = async () => await blob.DownloadToFileAsync(fileName);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public async Task DownloadToFileShouldNotThrowExceptionIfBlobContainerExists()
        {
            var blob = await GetABlobFromAValidBlobContainer();

            var fileName = Path.GetTempFileName();

            await blob.UploadFileAsync(fileName);

            Func<Task> action = async () => await blob.DownloadToFileAsync(fileName);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public async Task DownloadToFileShouldThrowExceptionIfBlobDoesNotExist()
        {
            var blob = await GetABlobFromAValidBlobContainer();

            var fileName = Path.GetTempFileName();

            Func<Task> action = async () => await blob.DownloadToFileAsync(fileName);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public async Task DownloadToFileShouldThrowExceptionIfFilePathIsNull()
        {
            var blob = await GetABlobFromAValidBlobContainer();

            await blob.UploadTextAsync("Blob Content");
            Func<Task> action = async () => await blob.DownloadToFileAsync(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task DownloadToFileShouldDownloadTheFileIfBlobContainerExists()
        {
            var blob = await GetABlobFromAValidBlobContainer();
            var fileName = Path.GetTempFileName();
            const string Filecontent = "FileContent";

            await blob.UploadTextAsync(Filecontent);
            await blob.DownloadToFileAsync(fileName);

            File.ReadAllText(fileName).Should().Be(Filecontent);
        }

        [Fact]
        public async Task DownloadToFileShouldCreateNewFileIfFilePathDoesNotExist()
        {
            var blob = await GetABlobFromAValidBlobContainer();

            const string Filecontent = "FileContent";

            await blob.UploadTextAsync(Filecontent);

            var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            await blob.DownloadToFileAsync(fileName);

            File.Exists(fileName).Should().BeTrue();
        }

        [Fact]
        public void UploadTextToBlobShouldNotUploadTextIfBlobContainerDoesNotExists()
        {
            var blobContainer = GetBlobContainer();

            var blobDirectory = GetBlobDirectory(blobContainer);

            var blob = GetBlob(blobDirectory);

            Func<Task> action = async () => await blob.UploadTextAsync(SampleBlobContent);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void DownloadTextFromBlobShouldThrowExceptionIfBlobContainerDoesNotExists()
        {
            var blobContainer = GetBlobContainer();

            var blobDirectory = GetBlobDirectory(blobContainer);

            var blob = GetBlob(blobDirectory);

            Func<Task> action = async () => await blob.DownloadTextAsync();

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void DownloadToStreamShouldThrowExceptionIfBlobContainerDoesNotExist()
        {
            var blobContainer = GetBlobContainer();
            var blob = blobContainer.GetBlob(SampleBlobName);

            Func<Task> action = async () => await blob.DownloadToStreamAsync(null);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public async Task DownloadToStreamShouldReturnCorrectStream()
        {
            var blob = await GetABlobFromAValidBlobContainer();
            await blob.UploadTextAsync(SampleBlobContent);

            var downloadedBlobContents = await GetBlobContentsFromStream(blob);

            downloadedBlobContents.Should().Be(SampleBlobContent);
        }

        [Fact]
        public async Task UploadTextToBlobShouldUploadTextIfBlobContainerExists()
        {
            var blob = await GetABlobFromAValidBlobContainer();
            await blob.UploadTextAsync(SampleBlobContent);

            (await blob.DownloadTextAsync()).Should().Be(SampleBlobContent);
            blob.LastModifiedTime.Should().BeWithin(1.Minutes()).Before(DateTime.UtcNow);
        }

        [Fact]
        public void UploadFromStreamShouldThrowExceptionIfBlobContainerDoesNotExist()
        {
            var blobContainer = GetBlobContainer();
            var blob = blobContainer.GetBlob(SampleBlobName);

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(SampleBlobContent)))
            {
                var stream = memoryStream;
                Func<Task> action = async () => await blob.UploadFromStreamAsync(stream);
                action.Should().Throw<Exception>();
            }
        }

        [Fact]
        public async Task UploadFromStreamShouldUploadIfBlobContainerExists()
        {
            var blob = await GetABlobFromAValidBlobContainer();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(SampleBlobContent)))
            {
                await blob.UploadFromStreamAsync(stream);
            }

            (await blob.DownloadTextAsync()).Should().Be(SampleBlobContent);
            blob.LastModifiedTime.Should().BeWithin(1.Minutes()).Before(DateTime.UtcNow);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (var action in _cleanupTasks)
                {
                    Task.Run(action).Wait();
                }
            }

            _disposed = true;
        }

        private static IBlob GetBlob(IBlobDirectory blobDirectory)
        {
            var blobName = AzureResourceUniqueNameCreator.CreateUniqueBlobName();

            var blob = blobDirectory.GetBlob(blobName);

            return blob;
        }

        private static async Task<string> GetBlobContentsFromStream(IBlob blob)
        {
            string blobContents;

            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream();

                await blob.DownloadToStreamAsync(memoryStream);
                memoryStream.Position = 0;

                using (var streamReader = new StreamReader(memoryStream))
                {
                    memoryStream = null;
                    blobContents = streamReader.ReadToEnd();
                }
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
            }

            return blobContents;
        }

        private static IBlobDirectory GetBlobDirectory(IBlobContainer blobContainer)
        {
            var directoryName = AzureResourceUniqueNameCreator.CreateUniqueBlobDirectoryName();
            var blobDirectory = blobContainer.GetBlobDirectory(directoryName);
            return blobDirectory;
        }

        private IBlobContainer GetBlobContainer()
        {
            var uniqueBlobContainerName = AzureResourceUniqueNameCreator.CreateUniqueBlobContainerName();
            var blobContainer = BlobClient.GetBlobContainer(uniqueBlobContainerName);
            _cleanupTasks.Add(() => blobContainer.DeleteIfExistsAsync());
            return blobContainer;
        }

        private async Task<IBlob> GetABlobFromAValidBlobContainer()
        {
            var blobContainer = GetBlobContainer();
            var blobDirectory = GetBlobDirectory(blobContainer);
            var blob = GetBlob(blobDirectory);
            await blobContainer.CreateAsync();

            return blob;
        }
    }
}
