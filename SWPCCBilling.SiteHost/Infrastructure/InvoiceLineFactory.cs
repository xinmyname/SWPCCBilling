using System;
using System.Collections.Generic;
using System.Linq;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class InvoiceLineFactory
    {
        private readonly LedgerStore _ledgerStore;
        private readonly IList<Fee> _cachedFees; 

        public InvoiceLineFactory(LedgerStore ledgerStore, FeeStore feeStore)
        {
            _ledgerStore = ledgerStore;
            
            // This might be a bad idea...
            _cachedFees = feeStore.LoadAll().ToList();
        }

        public IEnumerable<InvoiceLine> Generate(DateTime month, long familyId)
        {
            IList<LedgerLine> ledgerLines = _ledgerStore.Load(familyId).ToList();
            decimal pastDue = ledgerLines.Where(l => l.Date.ToSQLiteDate().Value < month).Sum(ledgerLine => (decimal) ledgerLine.Amount);

            if (pastDue > 0m)
                yield return new InvoiceLine("Past Due", pastDue);
            else if (pastDue < 0m)
                yield return new InvoiceLine("Credit", pastDue);

            foreach (LedgerLine ledgerLine in ledgerLines
                .Where(l => l.Date.ToSQLiteDate().Value >= month)
                .Where(l => l.Date.ToSQLiteDate().Value < month.AddMonths(1)))
            {
                string description = String.Empty;

                if (ledgerLine.PaymentId.HasValue)
                    description = "Payment";
                else if (ledgerLine.FeeId.HasValue)
                    description = _cachedFees.Single(f => f.Id == ledgerLine.FeeId).Name;

                if (!String.IsNullOrEmpty(ledgerLine.Notes))
                    description += " - " + ledgerLine.Notes;

                yield return new InvoiceLine(description, (decimal)ledgerLine.UnitPrice, ledgerLine.Quantity, (decimal)ledgerLine.Amount);
            }
        }
    }
}