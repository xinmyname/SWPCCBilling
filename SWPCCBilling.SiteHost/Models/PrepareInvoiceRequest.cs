namespace SWPCCBilling.Models
{
    public class PrepareInvoiceRequest
    {
        public long FamilyId { get; set; } 
        public string Month { get; set; } 
    }

    public class PrepareInvoiceReceipt
    {
        public int NumPrepared { get; set; }
    }
}