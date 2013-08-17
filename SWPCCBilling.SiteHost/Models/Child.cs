using System;
using SWPCCBilling.Infrastructure;

namespace SWPCCBilling.Models
{
    public class Child
    {
        public long Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Room { get; set; }
        public string Joined { get; set; }
        public string Departed { get; set; }
        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Wed { get; set; }
        public bool Thu { get; set; }
        public bool Fri { get; set; }
        public string Effective { get; set; }

        public const string ChildRoomYoungToddler = "YT";
        public const string ChildRoomToddler = "TR";
        public const string ChildRoomPreschool1 = "PS1";
        public const string ChildRoomPreschool2 = "PS2";

        public DateTime? JoinedDate
        {
            get { return Joined.ToSQLiteDateTime(); }
            set { Joined = value.HasValue ? value.Value.ToSQLiteDateTime() : null; }
        }

        public DateTime? DepartedDate
        {
            get { return Departed.ToSQLiteDateTime(); }
            set { Departed = value.HasValue ? value.Value.ToSQLiteDateTime() : null; }
        }

        public DateTime? EffectiveDate
        {
            get { return Effective.ToSQLiteDateTime(); }
            set { Effective = value.HasValue ? value.Value.ToSQLiteDateTime() : null; }
        }

        public Child()
        {
        }

        public Child(string firstName, string lastName, string room, DateTime joined, DateTime effectiveDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Room = room;
            JoinedDate = joined;
            EffectiveDate = effectiveDate;
        }
    }
}