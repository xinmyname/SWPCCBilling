using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;
using SWPCCBilling.ViewModels;

namespace SWPCCBilling.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(FamilyStore familyStore, FeeStore feeStore, LedgerStore ledgerStore, LedgerLineFactoryFactory lineFactoryFactory)
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
                var chargeRequest = this.Bind<ChargeRequest>();
                var receipt = new ChargeReceipt();
                Fee fee = feeStore.Load(chargeRequest.FeeId);
                LedgerLineFactory lineFactory = lineFactoryFactory.Create();

                foreach (long familyId in chargeRequest.GetFamilyIds())
                {
                    Family family = familyStore.Load(familyId);
                    LedgerLine line = lineFactory.CalculateCharge(chargeRequest, fee, family);

                    ledgerStore.Add(line);

                    receipt.NumFamilies++;
                    receipt.Amount += (decimal) line.Amount;
                }

                return Response.AsJson(receipt);
            };

            Post["/home/payment"] = _ =>
            {
                throw new NotImplementedException();
                //var paymentReceipt = new
                //{
                //    amount = 14.0m,
                //    family = "Test Family"
                //};

                //return Response.AsJson(paymentReceipt);
            };
        }

    }
}