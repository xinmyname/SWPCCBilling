using System;

namespace SWPCCBilling.Models
{
    public class LedgerLine
    {
        public long Id { get; set; }
        public long FamilyId { get; set; }
        public string Date { get; set; }
        public long? FeeId { get; set; }
        public long? PaymentId { get; set; }
        public double Amount { get; set; }
        public string Notes { get; set; }
    }

    public class LedgerLineReport
    {
        public string Date { get; set; }
        public string FamilyName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }

        public bool HasNotes
        {
            get { return !String.IsNullOrEmpty(Notes); }
        }
    }
}
