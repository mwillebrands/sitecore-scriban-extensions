using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Caching;

namespace Custom.XA.Feature.ScribanExtensions.Caching
{
    public class ScribanIncludeCache : CustomCache
    {
        public ScribanIncludeCache(string name, long maxSize) : base(name, maxSize)
        {
        }

        public ScribanIncludeCache(ICache innerCache) : base(innerCache)
        {
        }

        public void Set(string key, ScribanIncludeCacheItem value)
        {
            SetObject(key, value);
        }

        public ScribanIncludeCacheItem Get(string key)
        {
            return GetObject(key) as ScribanIncludeCacheItem;
        }
    }
}