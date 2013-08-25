using System;
using System.Collections.Generic;
using System.Linq;

namespace SWPCCBilling.Models
{
    public class ChargeRequest
    {
        public string FamilyList { get; set; }
        public DateTime ChargeDate { get; set; }
        public long FeeId { get; set; }
        public decimal UnitPrice { get; set; }
        public long Quantity { get; set; }
        public int ChargeMinutes { get; set; }
        public string ChargeNotes { get; set; }

        public IEnumerable<long> GetFamilyIds()
        {
            return FamilyList
                .Split(',')
                .Select(Int64.Parse);
        }
    }

    public class ChargeReceipt
    {
        public int NumFamilies { get; set; }
        public decimal Amount { get; set; }
    }
}