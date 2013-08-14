using System;

namespace SWPCCBilling.SiteHost.Models
{
    public class Child
    {
        public int Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Room { get; set; }
        public DateTime Joined { get; set; }
        public DateTime Departed { get; set; }
        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Wed { get; set; }
        public bool Thu { get; set; }
        public bool Fri { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}