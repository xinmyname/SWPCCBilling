using Nancy.Responses;

namespace SWPCCBilling.SiteHost
{
    public class StartupModule : Nancy.NancyModule
    {
        public StartupModule()
        {
            Get["/"] = _ => new EmbeddedFileResponse(GetType().Assembly, "SWPCCBilling.SiteHost.Content", "index.html");
        }
    }
}