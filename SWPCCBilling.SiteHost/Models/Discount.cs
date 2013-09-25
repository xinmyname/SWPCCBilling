namespace SWPCCBilling.Models
{
    public class Discount
    {
        public long Id { get; set; }
        public long FamilyId { get; set; }
        public long FeeId { get; set; } 
        public double Percent { get; set; } 
        public bool IsFinancialAid { get; set; }
    }
}