using Nancy.Responses;

namespace SWPCCBilling
{
    public class StartupModule : Nancy.NancyModule
    {
        public StartupModule()
        {
            Get["/"] = _ => new EmbeddedFileResponse(GetType().Assembly, "SWPCCBilling.Content", "index.html");
        }
    }
}