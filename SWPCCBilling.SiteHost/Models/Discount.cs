using System;

namespace SWPCCBilling.Models
{
    public class Discount
    {
        public int FeeId { get; set; } 
        public decimal Percent { get; set; } 
        public DateTime EffectiveDate { get; set; }
    }
}