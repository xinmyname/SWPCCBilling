using Nancy.Responses;

namespace SWPCCBilling
{
    public class HelloModule : Nancy.NancyModule
    {
        public HelloModule()
        {
            Get["/"] = _ => new EmbeddedFileResponse(GetType().Assembly, "SWPCCBilling.SiteHost.Content", "index.html");
        }
    }
}