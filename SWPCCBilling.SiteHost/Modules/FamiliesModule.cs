using Nancy;

namespace SWPCCBilling.Modules
{
    public class FamiliesModule : NancyModule
    {
        public FamiliesModule()
        {
            Get["/families"] = _ => View["Index"];
        }
    }
}