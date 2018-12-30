using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SSW.Ports.AzureStorage.Definition.Blobs;
using Xunit;

namespace SSW.Ports.AzureStorage.Definition.Tests.Blobs
{
    public abstract class BlobContainerTests : IDisposable
    {
        private readonly IList<Func<Task>> _cleanupTasks = new List<Func<Task>>();

        private bool _disposed = false;

        protected IBlobClient BlobClient { get; set; }

        [Fact]
        public async Task BlobContainerShouldBeAbleToBeDeletedEvenIfItHasBlobs()
        {
            var containerName = AzureResourceUniqueNameCreator.CreateUniqueBlobContainerName();
            var blobContainer = CreateBlobContainer(containerName);
            await blobContainer.CreateAsync();
            await blobContainer.GetBlob("blobName").UploadTextAsync("blobContent");

            await blobContainer.DeleteIfExistsAsync();

            Func<Task> action = async () => await blobContainer.GetBlob("blobName").UploadTextAsync("text");
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void BlobContainerCanGiveBlobEvenIfItDoesNotExist()
        {
            var containerName = AzureResourceUniqueNameCreator.CreateUniqueBlobContainerName();
            var blobContainer = CreateBlobContainer(containerName);

            var blobName = AzureResourceUniqueNameCreator.CreateUniqueBlobName();

            var blob = blobContainer.GetBlob(blobName);

            blob.Should().NotBeNull();
        }

        [Fact]
        public void GetBlobDirectoryShouldReturnBlobDirectoryInstanceFromContainerEvenDirectoryDoesNotExists()
        {
            var containerName = AzureResourceUniqueNameCreator.CreateUniqueBlobContainerName();
            var blobContainer = CreateBlobContainer(containerName);

            var directoryName = AzureResourceUniqueNameCreator.CreateUniqueBlobDirectoryName();

            var blobDirectory = blobContainer.GetBlobDirectory(directoryName);

            blobDirectory.Should().NotBeNull();
        }

        [Fact]
        public async Task IsBlobPermissionsPublicShouldReturnFalse()
        {
            var containerName = AzureResourceUniqueNameCreator.CreateUniqueBlobContainerName();
            var blobContainer = CreateBlobContainer(containerName);
            await blobContainer.CreateAsync();

            (await blobContainer.IsBlobPermissionsPublicAsync()).Should().BeFalse();
        }

        [Fact]
        public async Task SetBlobPermissionToPublicShouldSetItAppropriately()
        {
            var containerName = AzureResourceUniqueNameCreator.CreateUniqueBlobContainerName();
            var blobContainer = CreateBlobContainer(containerName);
            await blobContainer.CreateAsync();

            await blobContainer.SetBlobPermissionsToPublicAsync();

            (await blobContainer.IsBlobPermissionsPublicAsync()).Should().BeTrue();
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

        private IBlobContainer CreateBlobContainer(string containerName)
        {
            var c = BlobClient.GetBlobContainer(containerName);
            _cleanupTasks.Add(() => c.DeleteIfExistsAsync());

            return c;
        }
    }
}
