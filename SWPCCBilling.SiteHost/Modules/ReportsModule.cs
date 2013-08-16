using Nancy;

namespace SWPCCBilling.Modules
{
    public class ReportsModule : NancyModule
    {
        public ReportsModule()
        {
            Get["/reports"] = _ => View["Index"];
        }
    }
}