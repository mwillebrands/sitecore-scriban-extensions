using System.Threading.Tasks;
using Custom.XA.Feature.ScribanExtensions.Repositories;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace Custom.XA.Feature.ScribanExtensions.Scriban
{
    public class SitecoreTemplateLoader : ITemplateLoader
    {
        private readonly IScribanIncludeRepository _scribanIncludeRepository;

        public SitecoreTemplateLoader(IScribanIncludeRepository scribanIncludeRepository)
        {
            _scribanIncludeRepository = scribanIncludeRepository;
        }

        public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName)
        {
            return templateName;
        }

        public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath)
        {
            return _scribanIncludeRepository.GetInclude(templatePath);
        }

        public ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath)
        {
            return new ValueTask<string>(_scribanIncludeRepository.GetInclude(templatePath));
        }
    }
}
