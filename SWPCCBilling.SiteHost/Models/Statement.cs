using System;

namespace SWPCCBilling.Models
{
    class Statement
    {
        public string FamilyName { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public string CheckNum { get; set; }
        public DateTime DepositDate { get; set; }
        public decimal OverUnder { get; set; }

        public Statement()
        {
        }

        public Statement(string familyName)
        {
            FamilyName = familyName;
        }

        public string AmountDueText 
        {
            get { return AmountDue.ToString("F2"); }
        }

        public string AmountPaidText
        {
            get { return AmountDue.ToString("F2"); }
        }

        public string DepositDateText
        {
            get { return DepositDate.ToShortDateString(); }
        }

        public string OverUnderText
        {
            get { return AmountDue.ToString("F2"); }
        }

    }
}
