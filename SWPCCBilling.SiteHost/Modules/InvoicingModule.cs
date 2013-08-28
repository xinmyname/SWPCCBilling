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
        public InvoicingModule(ILog log, DefaultViewRenderer viewRenderer, FamilyStore familyStore, InvoiceLineFactory lineFactory, InvoiceStore invoiceStore)
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

                if (request.FamilyId == 0)
                {
                    foreach (Family family in familyStore.LoadAll())
                    {
                        Invoice invoice = factory.PrepareInvoice(invoiceDate, family);
                        invoiceStore.Save(invoice);
                    }
                }
                else
                {
                    Family family = familyStore.Load(request.FamilyId);
                    Invoice invoice = factory.PrepareInvoice(invoiceDate, family);
                    invoiceStore.Save(invoice);
                }
               
                return Response.AsJson(new {Success = false});
            };

            Post["/invoicing/send"] = _ =>
            {
                var request = this.Bind<SendInvoiceRequest>();


                return Response.AsJson(new { Success = false });
            };

            Get["/invoicing/test"] = _ =>
            {
                var invoiceDate = new DateTime(2013, 9, 1);
                var family = familyStore.Load(4);
                var dueDate = invoiceDate.AddDays(family.DueDay - 1);
                var lines = lineFactory.Generate(invoiceDate, family.Id).ToList();
                decimal amountDue = lines.Select(l => l.Amount).Aggregate(0m, (x, y) => x + y);

                var model = new
                {
                    InvoiceDate = invoiceDate.ToString("MMM yyyy"),
                    Family = family,
                    ParentEmails = family.Parents.Where(p => p.Email != null).Select(p => p.Email).ToList(),
                    DueDate = dueDate.ToString("MMMM d, yyyy"),
                    Lines = lines,
                    AmountDueText = amountDue.ToString("C")
                };
/*
                Response response = viewRenderer.RenderView(Context, "InvoiceTemplate_txt", model);
                var data = new MemoryStream();
                response.Contents(data);
                return Response.AsText(Encoding.UTF8.GetString(data.ToArray()));
*/
//                Response response = viewRenderer.RenderView(Context, "InvoiceTemplate", model);
                return View["InvoiceTemplate", model];
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