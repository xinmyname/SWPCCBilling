using System;
using System.IO;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.ViewEngines;
using SWPCCBilling.Infrastructure;

namespace SWPCCBilling.Modules
{
    public class InvoicingModule : NancyModule
    {
        public InvoicingModule(DefaultViewRenderer viewRenderer, FamilyStore familyStore, InvoiceLineFactory lineFactory)
        {
            Get["/invoicing"] = _ =>
            {
                return View["Index"];
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
    }
}