using System;
using System.IO;
using System.Linq;
using Nancy;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.ViewModels;

namespace SWPCCBilling.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(FamilyStore familyStore, FeeStore feeStore)
        {
            Get["/"] = _ =>
            {
                var model = new HomeViewModel
                {
                    Families = familyStore.LoadAll().ToList(),
                    Fees = feeStore.LoadAll().ToList(),
                    Today = DateTime.Today.ToShortDateString()
                };

                return View["Index", model];
            };

            Post["/home/charge"] = _ =>
            {
                var reader = new StreamReader(Request.Body);
                string body = reader.ReadToEnd();
                var chargeReceipt = new
                {
                    numFamilies = 2,
                    amount = 26.0m
                };

                return Response.AsJson(chargeReceipt);
            };

            Post["/home/payment"] = _ =>
            {
                var paymentReceipt = new
                {
                    amount = 14.0m,
                    family = "Test Family"
                };

                return Response.AsJson(paymentReceipt);
            };
        }
    }
}