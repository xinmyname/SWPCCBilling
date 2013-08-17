using Nancy;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;

namespace SWPCCBilling.Modules
{
    public class FeesModule : NancyModule
    {
        private const string FeeTypeFixed = "F";
        private const string FeeTypeVarying = "V";
        private const string FeeTypePerMinute = "P";

        public FeesModule(FeeStore feeStore)
        {
            Get["/fees"] = _ => View["Index", feeStore.LoadAll()];
            Get["/fees/add"] = _ =>
            {
                feeStore.Add(new Fee("New Fee", FeeTypeFixed, 0.0));
                return Response.AsRedirect("/fees");
            };
        }
    }
}