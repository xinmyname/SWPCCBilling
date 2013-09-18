namespace SWPCCBilling.Models
{
    public class Payment
    {
        public long Id { get; set; }
        public long FamilyId { get; set; }
        public string CheckNum { get; set; }
        public double Amount { get; set; }
        public string Received { get; set; }
        public string Deposited { get; set; }
    }
}