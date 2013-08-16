using Nancy;

namespace SWPCCBilling.Modules
{
    public class DiscountsModule : NancyModule
    {
        public DiscountsModule()
        {
            Get["/discounts"] = _ => View["Index"];
        }
    }
}