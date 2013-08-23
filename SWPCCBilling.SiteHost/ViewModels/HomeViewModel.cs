using System;
using System.Collections.Generic;
using SWPCCBilling.Models;

namespace SWPCCBilling.ViewModels
{
    public class HomeViewModel
    {
        public IList<Family> Families { get; set; }
        public IList<Fee> Fees { get; set; }
        public string Today { get; set; }
    }
}
