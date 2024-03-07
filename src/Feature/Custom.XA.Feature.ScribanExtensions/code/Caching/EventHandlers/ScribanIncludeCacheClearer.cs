using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.Caching.EventHandlers;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace Custom.XA.Feature.ScribanExtensions.Caching.EventHandlers
{
    public class ScribanIncludeCacheClearer : DatabaseCacheClearer
    {
        private static ID _variantScribanTemplateId = ID.Parse("{8FCD3CFE-8B3B-423E-8176-6A7C72CB43FC}");

        protected override void ClearCache(object sender, string name)
        {
            var cache = ServiceLocator.ServiceProvider.GetService<IScribanIncludeCacheManager>();
            cache?.ClearCache(name);
        }

        protected override void OnItemDeleted(object sender, Item item)
        {
            if (ShouldHandle(item))
            {
                base.OnItemDeleted(sender, item);
            }
        }

        protected override void OnItemSaved(object sender, Item item)
        {
            if (ShouldHandle(item))
            {
                base.OnItemSaved(sender, item);
            }
        }

        protected override void OnItemMoved(object sender, Item item)
        {
            if (ShouldHandle(item))
            {
                base.OnItemMoved(sender, item);
            }
        }

        private bool ShouldHandle(Item item)
        {
            return item.InheritsFrom(Constants.Templates.ScribanInclude.TemplateId) || item.InheritsFrom(_variantScribanTemplateId);
        }

    }
}
