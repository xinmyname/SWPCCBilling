using Nancy;

namespace SWPCCBilling.Modules
{
    public class LedgerModule : NancyModule
    {
        public LedgerModule()
        {
            Get["/ledger"] = _ => View["Index"];
        }
    }
}