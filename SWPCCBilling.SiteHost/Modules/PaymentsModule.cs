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
                var model = new List<PaymentViewModel>();

                foreach (Payment payment in paymentStore.LoadUndeposited())
                {
                    Family family = familyStore.Load(payment.FamilyId);
                    DateTime? received = payment.Received.ToSQLiteDateTime();

                    model.Add(new PaymentViewModel
                    {
                        PaymentId = payment.Id,
                        FamilyName = family.FamilyName,
                        CheckNum = payment.CheckNum,
                        AmountText = payment.Amount.ToString("C"),
                        ReceivedText = received.Value.ToShortDateString()
                    });
                }

                return View["Index", model.OrderBy(p => p.FamilyName)];
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
