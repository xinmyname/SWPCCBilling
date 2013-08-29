using System;

namespace SWPCCBilling.Models
{
    public class PaymentRequest
    {
        public int FamilyId { get; set; }
        public DateTime Date { get; set; }
        public string CheckNum { get; set; }
        public decimal Amount { get; set; } 
        public decimal Donation { get; set; }
        public string Notes { get; set; }
    }

    public class PaymentReceipt
    {
        public decimal Amount { get; set; }
        public string FamilyName { get; set; }
    }
}