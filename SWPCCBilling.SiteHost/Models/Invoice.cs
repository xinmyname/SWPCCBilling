using System;

namespace SWPCCBilling.Models
{
    public class Invoice
    {
        public DateTime Date { get; set; }
        public string FamilyName { get; set; }
        public string HTMLBody { get; set; } 
        public string TextBody { get; set; }

        public Invoice(DateTime date, string familyName)
        {
            Date = date;
            FamilyName = familyName;
        }
    }

    public enum InvoiceBodyType
    {
        HTML,
        Text
    }
}