using Sitecore.XA.Foundation.Caching;

namespace Custom.XA.Feature.ScribanExtensions.Caching
{
    public interface IScribanIncludeCacheManager
    {
        void Set(string database, string key, ScribanIncludeCacheItem value);
        ScribanIncludeCacheItem Get(string database, string key);
        void ClearCache(string database);
    }
}