using System;

namespace SWPCCBilling.ViewModels
{
    public class PaymentDepositViewModel
    {
        public long FamilyId { get; set; }
        public string FamilyName { get; set; }
        public DateTime? Date { get; set; }
        public string DateText { get; set; }
        public long? FeeId { get; set; }
        public string FeeName { get; set; }
        public decimal Amount { get; set; }
        public string AmountText { get; set; }
        public string Notes { get; set; }
    }
}