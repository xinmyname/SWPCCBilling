using System;

namespace SWPCCBilling.ViewModels
{
    public class DepositViewModel
    {
        public int Number { get; set; } 
        public DateTime When { get; set; }
        public decimal Amount { get; set; }

        public DepositViewModel(int number, DateTime? when, decimal amount)
        {
            if (!when.HasValue)
                throw new ArgumentNullException("when");

            Number = number;
            When = when.Value;
            Amount = amount;
        }

        public string WhenText
        {
            get { return When.ToShortDateString(); }
        }

        public string AmountText
        {
            get { return Amount.ToString("C"); }
        }
    }
}