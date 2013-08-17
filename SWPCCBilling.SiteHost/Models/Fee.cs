using System;

namespace SWPCCBilling.Models
{
    public class Fee
    {
        public long Id { get; set; } 
        public string Name { get; set; }
        public string Type { get; set; }
        public double? Amount { get; set; }

        public const string FeeTypeFixed = "F";
        public const string FeeTypeVarying = "V";
        public const string FeeTypePerChild = "C";
        public const string FeeTypePerDay = "D";
        public const string FeeTypePerMinute = "M";

        public Fee()
        {
        }

        public Fee(string name, string type, double amount)
        {
            Name = name;
            Type = type;
            Amount = amount;
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