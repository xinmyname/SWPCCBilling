using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using log4net;
using Nancy;
using Nancy.ModelBinding;
using Nancy.ViewEngines;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;
using SWPCCBilling.ViewModels;

namespace SWPCCBilling.Modules
{
    public class InvoicingModule : NancyModule
    {
        public InvoicingModule(
            ILog log, 
            DefaultViewRenderer viewRenderer, 
            FamilyStore familyStore, 
            InvoiceLineFactory lineFactory, 
            InvoiceStore invoiceStore, 
            InvoiceMailer invoiceMailer)
        {
            Get["/invoicing"] = _ =>
            {
                var families = familyStore.LoadAll();
                var nextMonth = DateTime.Now.AddMonths(1);

                var model = new
                {
                    Families = families,
                    Months = GetMonths(),
                    NextMonth = nextMonth.ToString("yyyy-MM"),
                    InvoiceLocation = invoiceStore.GetPathToAllInvoices(),
                    InvoiceUrl = new Uri(invoiceStore.GetPathToAllInvoices()).AbsoluteUri
                };

                return View["Index", model];
            };

            Post["/invoicing/prepare"] = _ =>
            {
                var request = this.Bind<PrepareInvoiceRequest>();
                var invoiceDate = DateTime.ParseExact(request.Month, "yyyy-MM", CultureInfo.InvariantCulture);
                var factory = new InvoiceFactory(log, viewRenderer, Context, lineFactory);
                var families = new List<Family>();
                var receipt = new PrepareInvoiceReceipt();

                if (request.FamilyId == 0)
                    families.AddRange(familyStore.LoadAll());
                else
                    families.Add(familyStore.Load(request.FamilyId));

                foreach (Family family in families)
                {
                    Invoice invoice = factory.PrepareInvoice(invoiceDate, family);
                    invoiceStore.Save(invoice);
                    receipt.NumPrepared++;
                }

                return Response.AsJson(receipt);
            };

            Post["/invoicing/send"] = _ =>
            {
                var request = this.Bind<SendInvoiceRequest>();
                var invoiceDate = DateTime.ParseExact(request.Month, "yyyy-MM", CultureInfo.InvariantCulture);
                var families = new List<Family>();
                var receipt = new SendInvoiceReceipt();

                if (request.FamilyId == 0)
                    families.AddRange(familyStore.LoadAll());
                else
                    families.Add(familyStore.Load(request.FamilyId));

                foreach (Family family in families)
                {
                    try
                    {
                        Invoice invoice = invoiceStore.Load(invoiceDate, family.Id, family.FamilyName);
                        invoiceMailer.Send(invoice, request.Password, family.Parents.Where(p => p.Email != null).Select(p => p.Email).ToList());
                    }
                    catch (Exception ex)
                    {
                        receipt.NumErrors++;
                        log.Error(ex);
                    }

                    receipt.NumSent++;
                }

                return Response.AsJson(receipt);
            };
        }

        private IEnumerable<MonthViewModel> GetMonths()
        {
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.AddMonths(-6).Year, 1, 1);

            for (int m = 0; m < 24; m++)
                yield return new MonthViewModel(startDate.AddMonths(m));
        }
    }

}