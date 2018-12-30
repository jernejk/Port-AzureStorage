using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SSW.Ports.AzureStorage.Definition.Blobs;
using Xunit;

namespace SSW.Ports.AzureStorage.Definition.Tests.Blobs
{
    public abstract class BlobClientTests : IDisposable
    {
        private readonly IList<Func<Task>> _cleanupTasks = new List<Func<Task>>();

        private bool _disposed = false;

        protected IBlobClient BlobClient { get; set; }

        protected string BlobClientBaseAddress { get; set; }

        [Fact]
        public async Task DoesBlobContainerExistsShouldReturnFalseIfBlobContainerDoesNotExists()
        {
            var containerName = AzureResourceUniqueNameCreator.CreateUniqueBlobContainerName();

            var blobContainerStatus = await BlobClient.DoesBlobContainerExistAsync(containerName);

            blobContainerStatus.Should().Be(false);
        }

        [Fact]
        public void GetBlobContainerOnBlobClientShouldReturnValidInstanceOfBlobContainer()
        {
            var blobContainer = BlobClient.GetBlobContainer("testforvalidblobcontainer");

            blobContainer.Should().NotBeNull();
        }

        [Fact]
        public async Task BlobContainerShouldCreateContainerEvenIfAlreadyCreated()
        {
            var c = CreateBlobContainer();
            await c.CreateAsync();

            Func<Task> action = async () => await BlobClient.GetBlobContainer(c.Name).CreateAsync();

            action.Should().NotThrow();
        }

        [Fact]
        public void GetBaseAddressShouldReturnBaseUriOfUnderlyingBlobClient()
        {
            BlobClient.GetBaseAddress().Should().Be(BlobClientBaseAddress);
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

        private IBlobContainer CreateBlobContainer()
        {
            var containerName = AzureResourceUniqueNameCreator.CreateUniqueBlobContainerName();
            var c = BlobClient.GetBlobContainer(containerName);
            _cleanupTasks.Add(() => c.DeleteIfExistsAsync());

            return c;
        }
    }
}
