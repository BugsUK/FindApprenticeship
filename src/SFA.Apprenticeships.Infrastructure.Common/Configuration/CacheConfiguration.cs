namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    public class CacheConfiguration
    {
        public const string AzureCacheName = "AzureCacheService";
        public const string MemoryCacheName = "MemoryCacheService";

        public bool UseCache { get; set; }

        public string DefaultCache { get; set; }
    }
}
