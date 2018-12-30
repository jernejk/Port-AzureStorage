using System;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace SSW.Ports.AzureStorage.Adapter.Azure
{
    public static class RetryPolicyFactory
    {
        public static IRetryPolicy CreateExponentialRetryPolicy()
        {
            return new ExponentialRetry(TimeSpan.Zero, 2);
        }
    }
}
