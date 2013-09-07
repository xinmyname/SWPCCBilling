using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Nancy;
using SWPCCBilling.Infrastructure;

namespace SWPCCBilling.Modules
{
    public class PaymentsModule : NancyModule
    {
        public PaymentsModule(ILog log, PaymentStore paymentStore)
        {
            Get["/payments"] = _ =>
            {

                return View["Index"];
            };

            Post["/payments/deposit"] = _ => Response.AsRedirect("/payments");
        }
    }
}
