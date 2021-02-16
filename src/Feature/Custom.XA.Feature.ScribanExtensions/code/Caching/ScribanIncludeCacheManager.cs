using Sitecore;
using Sitecore.Configuration;
using Sitecore.XA.Foundation.Caching;

namespace Custom.XA.Feature.ScribanExtensions.Caching
{
    public class ScribanIncludeCacheManager : IScribanIncludeCacheManager
    {
        public const string Name = "ScribanIncludeCache";
        public const string CacheSizeSetting = "Custom.XA.Feature.ScribanExtensions.Includes.CacheSize";

        private static DatabaseCacheManager<ScribanIncludeCache> Cache { get; }

        static ScribanIncludeCacheManager()
        {
            var size = StringUtil.ParseSizeString(Settings.GetSetting(CacheSizeSetting, "50MB"));
            Cache = new DatabaseCacheManager<ScribanIncludeCache>(Name, size);
        }

        public void Set(string database, string key, ScribanIncludeCacheItem value)
        {
            Cache.GetCacheForDatabase(database).Set(key, value);
        }

        public ScribanIncludeCacheItem Get(string database, string key)
        {
            return Cache.GetCacheForDatabase(database).Get(key);
        }

        public void ClearCache(string database)
        {
            Cache.Clear(database);
        }
    }
}
