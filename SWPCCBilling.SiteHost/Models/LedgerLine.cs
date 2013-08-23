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
}
