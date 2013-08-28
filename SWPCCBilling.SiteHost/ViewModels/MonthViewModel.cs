using System;

namespace SWPCCBilling.ViewModels
{
    public class MonthViewModel
    {
        public DateTime Date { get; set; }
        public string Value { get; set; }
        public string Display { get; set; }

        public MonthViewModel(DateTime date)
        {
            Date = date;
            Value = date.ToString("yyyy-MM");
            Display = date.ToString("MMMM yyyy");
        }
    }
}