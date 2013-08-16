using Nancy;

namespace SWPCCBilling.Modules
{
    public class InvoicingModule : NancyModule
    {
        public InvoicingModule()
        {
            Get["/invoicing"] = _ => View["Index"];
        }
    }
}