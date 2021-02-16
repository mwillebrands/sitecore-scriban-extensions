using System;
using System.Collections.Generic;
using System.Linq;
using Custom.XA.Feature.ScribanExtensions.Caching;
using Custom.XA.Feature.ScribanExtensions.Constants.Templates;
using Sitecore.Configuration;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.Multisite;
using Sitecore.XA.Foundation.Presentation;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Caching;

namespace Custom.XA.Feature.ScribanExtensions.Repositories
{
    public class ScribanIncludeRepository : IScribanIncludeRepository
    {
        private readonly ISharedSitesContext _sharedSitesContext;
        private readonly IContext _context;
        private readonly IPresentationContext _presentationContext;
        private readonly IScribanIncludeCacheManager _cacheManager;
        private readonly bool _outputMissingInclude;

        private const string OutputMissingIncludeSetting = "Custom.XA.Feature.ScribanExtensions.Includes.OutputMissing";

        public ScribanIncludeRepository(ISharedSitesContext sharedSitesContext,
            IContext context,
            IPresentationContext presentationContext,
            IScribanIncludeCacheManager cacheManager)
        {
            _sharedSitesContext = sharedSitesContext;
            _context = context;
            _presentationContext = presentationContext;
            _cacheManager = cacheManager;
            _outputMissingInclude = Settings.GetBoolSetting(OutputMissingIncludeSetting, true);
        }

        public virtual string GetInclude(string key)
        {
            key = key.ToLowerInvariant().Trim();
            var includeItem = _cacheManager.Get(_context.Database.Name, key) ?? GetIncludeAndPopulateCache(key);
            if (includeItem.Script == null && _outputMissingInclude)
            {
                return $"Scriban include '{key}' is missing";
            }

            return includeItem.Script ?? string.Empty;
        }

        protected virtual ScribanIncludeCacheItem GetIncludeAndPopulateCache(string key)
        {
            //Check the current site first
            var scribanInclude = SearchSiteForIncludeAndPopulateCache(_context.Item, key);
            if (scribanInclude != null)
            {
                return scribanInclude;
            }

            //Check the shared sites
            foreach (var site in _sharedSitesContext.GetSharedSitesWithoutCurrent(_context.Item))
            {
                scribanInclude = SearchSiteForIncludeAndPopulateCache(site, key);
                if (scribanInclude != null)
                {
                    return scribanInclude;
                }
            }

            var cacheItem = new ScribanIncludeCacheItem(null);
            _cacheManager.Set(_context.Database.Name, key, cacheItem);
            return cacheItem;
        }

        protected virtual ScribanIncludeCacheItem SearchSiteForIncludeAndPopulateCache(Item siteItem, string requestedKey)
        {
            var presentationItem = _presentationContext.GetPresentationItem(siteItem);
            var scribanIncludeFolder = presentationItem?.FirstChildInheritingFrom(ScribanIncludeFolder.TemplateId);
            var scribanIncludes = scribanIncludeFolder?
                .Axes
                .GetDescendants()
                .Where(x => x.InheritsFrom(ScribanInclude.TemplateId))
                .ToList() ?? new List<Item>();

            ScribanIncludeCacheItem result = null;
            foreach (var includeItem in scribanIncludes)
            {
                var includeItemKey = includeItem[ScribanInclude.Fields.Key].ToLowerInvariant().Trim();
                var includeItemScript = includeItem[ScribanInclude.Fields.Script];
                var cacheItem = new ScribanIncludeCacheItem(includeItemScript);
                _cacheManager.Set(_context.Database.Name, includeItemKey, cacheItem);

                if (includeItemKey.Equals(requestedKey))
                {
                    result = cacheItem;
                }
            }

            return result;
        }
    }
}
