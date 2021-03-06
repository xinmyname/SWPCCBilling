﻿using System;

namespace SWPCCBilling.Models
{
    public class Parent
    {
        public long Id { get; set; }
        public long FamilyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName).Trim(); }
        }

        public Parent()
        {
        }

        public Parent(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Parent(long familyId, string firstName, string lastName)
        {
            FamilyId = familyId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}