namespace SWPCCBilling.ViewModels
{
    public class PaymentViewModel
    {
        public long PaymentId { get; set; }
        public string FamilyName { get; set; }
        public string CheckNum { get; set; }
        public string AmountText { get; set; }
        public string ReceivedText { get; set; }
    }
}