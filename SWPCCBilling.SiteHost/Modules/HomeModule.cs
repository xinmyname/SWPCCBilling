using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;

namespace SWPCCBilling.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(FamilyStore familyStore, FeeStore feeStore, LedgerStore ledgerStore, LedgerLineFactoryFactory lineFactoryFactory)
        {
            Get["/"] = _ =>
            {
                var nextMonth = DateTime.Now.AddMonths(1);
                nextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);

                var model = new
                {
                    Families = familyStore.LoadAll().ToList(),
                    Fees = feeStore.LoadAll().ToList(),
                    NextMonth = nextMonth.ToShortDateString(),
                    Today = DateTime.Now.ToShortDateString()
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