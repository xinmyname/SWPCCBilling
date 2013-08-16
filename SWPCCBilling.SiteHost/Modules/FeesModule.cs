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

        private readonly FeeStore _feeStore;


        public FeesModule(FeeStore feeStore)
        {
            _feeStore = feeStore;
            Get["/fees"] = _ => Response.AsJson(_feeStore.LoadAll());
            Get["/fees/add"] = _ =>
            {
                var newFee = new Fee(0, "New Fee", FeeTypeFixed, 0.0);
                _feeStore.Add(newFee);
                return Response.AsJson(newFee);
            };
        }
    }
}