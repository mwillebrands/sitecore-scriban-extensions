using System;
using Sitecore.XA.Foundation.Scriban.Pipelines.GenerateScribanContext;
using Scriban.Runtime;
using Scriban.Syntax;

namespace Custom.XA.Feature.ScribanExtensions.Pipelines.GenerateScribanContext
{
    public class AddTemplateLoader : IGenerateScribanContextProcessor
    {
        private readonly ITemplateLoader _templateLoader;

        public AddTemplateLoader(ITemplateLoader templateLoader)
        {
            _templateLoader = templateLoader;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Import("se_param", new Func<ScriptArray, string, object>((arguments, key) =>
            {
                foreach (var arg in arguments)
                {
                    if (arg is ScriptNamedArgument namedArg && namedArg.Name == key)
                    {
                        return namedArg.Value;
                    }
                }
                return "";
            }));
            args.TemplateContext.TemplateLoader = _templateLoader;
        }
    }
}
