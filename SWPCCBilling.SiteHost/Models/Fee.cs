﻿using System;

namespace SWPCCBilling.Models
{
    public class Fee
    {
        public long Id { get; set; } 
        public string Name { get; set; }
        public string Type { get; set; }
        public double? Amount { get; set; }
        public string Category { get; set; }

        public const string FeeTypeFixed = "F";
        public const string FeeTypeVarying = "V";
        public const string FeeTypePerChild = "C";
        public const string FeeTypePerChildDay = "D";
        public const string FeeTypePerMinute = "M";

        public Fee()
        {
        }

        public Fee(string name, string type, double amount, string category)
        {
            Name = name;
            Type = type;
            Amount = amount;
            Category = category;
        }

        public string AmountText
        {
            get
            {
                return String.Format("{0:0.00}", Amount);
            }
        }
    }
}