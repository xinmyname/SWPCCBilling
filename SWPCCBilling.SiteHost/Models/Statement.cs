using System;
using System.Diagnostics.Eventing.Reader;

namespace SWPCCBilling.Models
{
    class Statement
    {
        public long FamilyId { get; set; }
        public string FamilyName { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Donated { get; set; }
        public string CheckNum { get; set; }
        public DateTime? DepositDate { get; set; }

        public Statement()
        {
        }

        public Statement(long familyId, string familyName, decimal amountDue)
        {
            FamilyId = familyId;
            FamilyName = familyName;
            AmountDue = amountDue;
        }

        public void AddPayment(string checkNum, decimal amount, DateTime? depositDate)
        {
            if (!String.IsNullOrEmpty(CheckNum))
                CheckNum += String.Format(", {0}", checkNum);
            else
                CheckNum = checkNum;

            AmountPaid += amount;
            DepositDate = depositDate;
        }

        public void AddDonation(decimal amount)
        {
            Donated += amount;
        }

        public string AmountDueText 
        {
            get { return AmountDue.ToString("C"); }
        }

        public string AmountPaidText
        {
            get { return AmountPaid.ToString("C"); }
        }

        public string DonatedText
        {
            get { return Donated.ToString("C"); }
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

        public decimal Balance
        {
            get { return AmountPaid - AmountDue - Donated; }
        }

        public string BalanceText
        {
            get { return Balance.ToString("C"); }
        }

        public string BalanceHtml
        {
            get
            {
                decimal balance = Balance;

                if (balance < 0)
                    return String.Format("<span style='color:#f00'>{0:C}</span>", balance);

                if (balance > 0)
                    return String.Format("<strong>{0:C}</strong>", balance);

                return balance.ToString("C"); 
            }
        }

    }
}
