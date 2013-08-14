using System;

namespace SWPCCBilling.SiteHost.Models
{
    public class Discount
    {
        public int FeeId { get; set; } 
        public decimal Percent { get; set; } 
        public DateTime EffectiveDate { get; set; }
    }
}