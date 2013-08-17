using System;
using SWPCCBilling.Infrastructure;

namespace SWPCCBilling.Models
{
    public class Discount
    {
        public int FamilyId { get; set; }
        public int FeeId { get; set; } 
        public double Percent { get; set; } 
        public string Effective { get; set; }

        public DateTime? EffectiveDate
        {
            get { return Effective.ToSQLiteDateTime(); }
            set { Effective = value.HasValue ? value.Value.ToSQLiteDateTime() : null; }
        }
    }
}