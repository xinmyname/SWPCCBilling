using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;
using SWPCCBilling.ViewModels;

namespace SWPCCBilling.Modules
{
    public class FeesModule : NancyModule
    {
        public FeesModule(FeeStore feeStore)
        {
            Get["/fees"] = _ => View["Index", feeStore.LoadAll()];

            Get["/fees/add"] = _ =>
            {
                Fee fee = feeStore.Add(new Fee("New Fee", Fee.FeeTypeFixed, 0.0, "None"));
                return Response.AsRedirect("/fees/" + fee.Id);
            };

            Get["/fees/{id}"] = _ =>
            {
                var allFees = feeStore.LoadAll().ToList();
                var fee = allFees.Single(f => f.Id == _.id);
                var feeEditor = new FeeEditorViewModel
                {
                    AllFees = allFees,
                    Fee = fee
                };
                return View["Edit", feeEditor];
            };

            Post["/fees/{id}"] = _ =>
            {
                Fee fee = this.Bind();
                feeStore.Save(fee);
                return Response.AsRedirect("/fees");
            };
        }
    }
}