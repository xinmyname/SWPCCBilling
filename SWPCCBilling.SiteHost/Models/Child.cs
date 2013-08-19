using System;
using System.Text;
using SWPCCBilling.Infrastructure;

namespace SWPCCBilling.Models
{
    public class Child
    {
        public long Id { get; set; }
        public long FamilyId { get; set; }
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

        public string Days
        {
            get
            {
                var days = new StringBuilder();

                if (Mon) days.Append("Mon");
                if (Tue) days.Append("Tue");
                if (Wed) days.Append("Wed");
                if (Thu) days.Append("Thu");
                if (Fri) days.Append("Fri");
                if (days.Length == 0)
                    return "None";

                int numDays = days.Length/3;

                for (int i = 0; i < numDays-1; i++)
                    days.Insert(i*3+3, ',');

                return days.ToString();
            }
        }

        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName).Trim(); }
        }

        public const string ChildRoomYoungToddler = "YT";
        public const string ChildRoomToddler = "TR";
        public const string ChildRoomPreschool1 = "PS1";
        public const string ChildRoomPreschool2 = "PS2";

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

        public Child(long familyId, string firstName, string lastName, string room, DateTime joined, DateTime effectiveDate)
        {
            FamilyId = familyId;
            FirstName = firstName;
            LastName = lastName;
            Room = room;
            JoinedDate = joined;
            EffectiveDate = effectiveDate;
        }
    }
}