using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Nancy;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;
using SWPCCBilling.ViewModels;

namespace SWPCCBilling.Modules
{
    public class PaymentsModule : NancyModule
    {
        public PaymentsModule(ILog log, PaymentStore paymentStore, FamilyStore familyStore)
        {
            Get["/payments"] = _ =>
            {
                var undepositedPayments = new List<PaymentViewModel>();
                var undepositedTotal = 0.0m;

                foreach (Payment payment in paymentStore.LoadUndeposited())
                {
                    Family family = familyStore.Load(payment.FamilyId);
                    DateTime? received = payment.Received.ToSQLiteDateTime();

                    undepositedPayments.Add(new PaymentViewModel
                    {
                        PaymentId = payment.Id,
                        FamilyName = family.FamilyName,
                        CheckNum = payment.CheckNum,
                        AmountText = payment.Amount.ToString("C"),
                        ReceivedText = received.Value.ToShortDateString()
                    });

                    undepositedTotal += (decimal) payment.Amount;
                }

                return View["Index", new
                {
                    UndepositedPayments = undepositedPayments.OrderBy(p => p.FamilyName),
                    UndepositedTotalText = undepositedTotal.ToString("C")
                }];
            };

            Post["/payments/deposit"] = _ =>
            {
                string allPaymentIds = Request.Form.PaymentIds;
                var paymentIds = allPaymentIds.Split(',').Select(Int64.Parse);

                paymentStore.MarkAsDeposited(paymentIds);

                return Response.AsRedirect("/payments");
            };
        }
    }
}
