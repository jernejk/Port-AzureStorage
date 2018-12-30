using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSW.Ports.AzureStorage.Definition.Blobs
{
    public interface IBlobDirectory
    {
        string Name { get; }

        IBlob GetBlob(string blobName);

        Task<IEnumerable<IBlob>> ListBlobsAsync(string blobNamePrefix, int pageSize, DateTime lastAccessedTime);
    }
}
