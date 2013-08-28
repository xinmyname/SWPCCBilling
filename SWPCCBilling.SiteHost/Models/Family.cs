using System;
using System.Collections.Generic;
using SWPCCBilling.Infrastructure;

namespace SWPCCBilling.Models
{
    public class Family
    {
        public long Id { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string Joined { get; set; }
        public string Departed { get; set; }
        public long DueDay { get; set; }
        public string Notes { get; set; }

        public IList<Parent> Parents { get; set; }
        public IList<Child> Children { get; set; }

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

        public bool IsReturning
        {
            get
            {
                if ((DateTime.Today - JoinedDate.Value).TotalDays > 200)
                    return true;

                return false;
            }
        }

        public string FamilyName
        {
            get
            {
                if (Parents.Count == 0)
                    return "Unknown Family";
                if (Parents.Count == 1)
                    return Parents[0].LastName + " Family";
                if (Parents[0].LastName == Parents[1].LastName)
                    return Parents[0].LastName + " Family";
                return String.Format("{0}/{1} Family",
                    Parents[0].LastName, Parents[1].LastName);
            }
        }

        public int NumChildren
        {
            get { return Children.Count; }
        }

        public int ChildDays
        {
            get
            {
                int childDays = -1;

                foreach (Child child in Children)
                {
                    childDays += child.Mon ? 1 : 0;
                    childDays += child.Tue ? 1 : 0;
                    childDays += child.Wed ? 1 : 0;
                    childDays += child.Thu ? 1 : 0;
                    childDays += child.Fri ? 1 : 0;
                }

                return childDays;
            }
        }

        public Family()
        {
            Parents = new List<Parent>();
            Children = new List<Child>();
        }

        public Family(string streetAddress, string city, string state, string zip, DateTime joined, long dueDay)
            : this()
        {
            StreetAddress = streetAddress;
            City = city;
            State = state;
            ZIP = zip;
            JoinedDate = joined;
            DueDay = dueDay;
        }

    }
}