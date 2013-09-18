using System;

namespace SWPCCBilling.Models
{
    public class Invoice
    {
        public DateTime Date { get; set; }
        public long FamilyId { get; set; }
        public string FamilyName { get; set; }
        public decimal AmountDue { get; set; }
        public string HTMLBody { get; set; } 
        public string TextBody { get; set; }

        public Invoice(DateTime date, long familyId, string familyName)
        {
            Date = date;
            FamilyId = familyId;
            FamilyName = familyName;
        }
    }

    public enum InvoiceBodyType
    {
        HTML,
        Text
    }
}