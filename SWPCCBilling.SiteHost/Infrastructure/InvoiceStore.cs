using System;
using System.Data;
using System.IO;
using log4net;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class InvoiceStore
    {
        private readonly ILog _log;
        private readonly DatabaseFactory _dbFactory;

        public InvoiceStore(ILog log, DatabaseFactory dbFactory)
        {
            _log = log;
            _dbFactory = dbFactory;
        }

        public Invoice Load(DateTime date, long familyId, string familyName)
        {
            string month = date.ToSQLiteDate();

            IDbConnection con = _dbFactory.OpenDatabase();

            var numInvoices = con.ExecuteScalar<long>("SELECT COUNT(*) FROM Invoice WHERE FamilyId=? AND Date=?",
                                               new { familyId, month });

            if (numInvoices == 0)
                return null;

            var amountDue = con.ExecuteScalar<double>("SELECT Amount FROM Invoice WHERE FamilyId=? AND Date=?",
                                               new { familyId, month });

            con.Close();

            string path = GetPathToInvoice(date, familyName, InvoiceBodyType.HTML);

            if (!File.Exists(path))
                return null;

            return new Invoice(date, familyId, familyName) 
            { 
                HTMLBody = File.ReadAllText(path),
                AmountDue = (decimal)amountDue
            };
        }

        public void Save(Invoice invoice)
        {
            string path = GetPathToInvoice(invoice.Date, invoice.FamilyName, InvoiceBodyType.HTML);

            _log.InfoFormat("Saving {0}", path);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllText(path, invoice.HTMLBody);

            IDbConnection con = _dbFactory.OpenDatabase();

            string invoiceDate = invoice.Date.ToSQLiteDate();

            int rows = con.Execute("UPDATE Invoice SET Amount=? WHERE FamilyId=? AND Date=?",
                new {invoice.AmountDue, invoice.FamilyId, invoiceDate});

            if (rows == 0)
            {
                con.Execute("INSERT INTO Invoice (FamilyId,Date,Amount) VALUES (?,?,?)",
                    new {invoice.FamilyId, invoice.Date, invoice.AmountDue});
            }

            con.Close();
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
            string dbPath = Properties.Settings.Default.InvoicePath;
            string invoicePath = String.Format("{0}{1}{2}", 
                Path.GetDirectoryName(dbPath),
                Path.DirectorySeparatorChar,
                "Invoices");

            return invoicePath;
        }
    }
}