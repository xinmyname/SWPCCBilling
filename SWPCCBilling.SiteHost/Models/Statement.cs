using System;

namespace SWPCCBilling.Models
{
    class Statement
    {
        public string FamilyName { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public string CheckNum { get; set; }
        public DateTime? DepositDate { get; set; }

        public Statement()
        {
        }

        public Statement(string familyName, decimal amountDue)
        {
            FamilyName = familyName;
            AmountDue = amountDue;
        }

        public void AddPayment(string checkNum, decimal amount)
        {
            if (!String.IsNullOrEmpty(CheckNum))
                CheckNum += String.Format(", {0}", checkNum);
            else
                CheckNum = checkNum;

            AmountPaid += amount;
        }

        public string AmountDueText 
        {
            get { return AmountDue.ToString("C"); }
        }

        public string AmountPaidText
        {
            get { return AmountPaid.ToString("C"); }
        }

        public string DepositDateText
        {
            get
            {
                return DepositDate.HasValue
                    ? DepositDate.Value.ToShortDateString()
                    : "";
            }
        }

        public decimal OverUnder
        {
            get { return AmountPaid - AmountDue; }
        }

        public string OverUnderText
        {
            get { return OverUnder.ToString("C"); }
        }

        public string OverUnderHtml
        {
            get
            {
                decimal overUnder = OverUnder;
                return overUnder < 0
                    ? String.Format("<span style='color:#f00'>{0:C}</span>", overUnder)
                    : overUnder.ToString("C"); 
            }
        }

    }
}
