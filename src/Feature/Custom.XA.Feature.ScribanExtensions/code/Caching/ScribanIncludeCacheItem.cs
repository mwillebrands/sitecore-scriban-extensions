namespace Custom.XA.Feature.ScribanExtensions.Caching
{
    public class ScribanIncludeCacheItem
    {
        public string Script { get; set; }

        public ScribanIncludeCacheItem(string script)
        {
            Script = script;
        }
    }
}