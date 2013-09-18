namespace SWPCCBilling.Models
{
    public class SendInvoiceRequest
    {
        public long FamilyId { get; set; }
        public string Month { get; set; }
        public string Password { get; set; }
    }

    public class SendInvoiceReceipt
    {
        public int NumSent { get; set; }
        public int NumErrors { get; set; }
    }
}