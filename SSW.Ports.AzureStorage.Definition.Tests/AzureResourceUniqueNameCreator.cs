using System;
using System.Globalization;
using System.IO;

namespace SSW.Ports.AzureStorage.Definition.Tests
{
    public static class AzureResourceUniqueNameCreator
    {
        public static string CreateUniqueBlobContainerName()
        {
            return "c" + Path.GetRandomFileName().Replace(".", string.Empty, StringComparison.Ordinal).ToLowerInvariant();
        }

        public static string CreateUniqueBlobName()
        {
            return "B" + Path.GetRandomFileName().Replace(".", string.Empty, StringComparison.Ordinal);
        }

        public static string CreateUniqueBlobDirectoryName()
        {
            return "D" + Path.GetRandomFileName().Replace(".", string.Empty, StringComparison.Ordinal);
        }

        public static string CreateUniqueConnectionString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                Guid.NewGuid(),
                Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
        }
    }
}
