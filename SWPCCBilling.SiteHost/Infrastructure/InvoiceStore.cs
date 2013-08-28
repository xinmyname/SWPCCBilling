using System;
using System.IO;
using log4net;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class InvoiceStore
    {
        private readonly ILog _log;

        public InvoiceStore(ILog log)
        {
            _log = log;
        }

        public Invoice Load(DateTime date, string familyName)
        {
            string path = GetPathToInvoice(date, familyName, InvoiceBodyType.HTML);

            return new Invoice(date, familyName) 
                { HTMLBody = File.ReadAllText(path) };
        }

        public void Save(Invoice invoice)
        {
            string path = GetPathToInvoice(invoice.Date, invoice.FamilyName, InvoiceBodyType.HTML);

            _log.InfoFormat("Saving {0}", path);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllText(path, invoice.HTMLBody);
        }

        public string GetPathToInvoice(DateTime date, string familyName, InvoiceBodyType bodyType)
        {
            familyName = familyName
                .Replace(" Family", "")
                .Replace("/", "");

            string ext = bodyType == InvoiceBodyType.HTML ? ".htm" : ".txt";

            return String.Format("{0}{1}{2:MMMM}{1}{3}{2:MMMyy}{4}",
                GetPathToAllInvoices(),
                Path.DirectorySeparatorChar,
                date,
                familyName,
                ext);
        }

        public string GetPathToAllInvoices()
        {
            string dbPath = Properties.Settings.Default.DatabasePath;
            string invoicePath = String.Format("{0}{1}{2}", 
                Path.GetDirectoryName(dbPath),
                Path.DirectorySeparatorChar,
                "Invoices");

            return invoicePath;
        }
    }
}