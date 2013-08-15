using Nancy.Responses;

namespace SWPCCBilling.Modules
{
    public class StartupModule : Nancy.NancyModule
    {
        public StartupModule()
        {
            Get["/"] = _ => new EmbeddedFileResponse(GetType().Assembly, "SWPCCBilling.Content", "index.html");
        }
    }
}