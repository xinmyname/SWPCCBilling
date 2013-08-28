using System;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using Nancy;
using Nancy.ViewEngines;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class InvoiceFactory
    {
        private readonly ILog _log;
        private readonly IViewRenderer _viewRenderer;
        private readonly NancyContext _context;
        private readonly InvoiceLineFactory _lineFactory;

        public InvoiceFactory(ILog log, IViewRenderer viewRenderer, NancyContext context, InvoiceLineFactory lineFactory)
        {
            _log = log;
            _viewRenderer = viewRenderer;
            _context = context;
            _lineFactory = lineFactory;
        }

        public Invoice PrepareInvoice(DateTime invoiceDate, Family family)
        {
            _log.InfoFormat("Preparing {0:MMMM yyyy} invoice for {1} ", invoiceDate, family.FamilyName);

            var invoice = new Invoice(invoiceDate, family.FamilyName);

            var dueDate = invoiceDate.AddDays(family.DueDay - 1);
            var lines = _lineFactory.Generate(invoiceDate, family.Id).ToList();
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

            var data = new MemoryStream();
            Response response = _viewRenderer.RenderView(_context, "InvoiceTemplate", model);
            response.Contents(data);

            invoice.HTMLBody = Encoding.UTF8.GetString(data.ToArray());

            data = new MemoryStream();
            response = _viewRenderer.RenderView(_context, "InvoiceTemplate_txt", model);
            response.Contents(data);

            invoice.TextBody = Encoding.UTF8.GetString(data.ToArray());

            return invoice;
        }
    }
}