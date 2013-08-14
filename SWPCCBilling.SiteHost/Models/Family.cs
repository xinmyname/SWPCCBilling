using System;
using System.Collections.Generic;

namespace SWPCCBilling.SiteHost.Models
{
    public class Family
    {
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public DateTime Joined { get; set; }
        public DateTime Departed { get; set; }
        public int DueDay { get; set; }
        public string Notes { get; set; }

        public IList<Parent> Parents { get; set; }
        public IList<Child> Children { get; set; }
        public IList<Discount> Discounts { get; set; }
    }
}