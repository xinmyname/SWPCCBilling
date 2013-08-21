using System.Collections.Generic;
using SWPCCBilling.Models;

namespace SWPCCBilling.ViewModels
{
    public class DiscountViewEditorModel
    {
        public IList<DiscountViewModel> AllDiscounts { get; set; }
        public IList<Family> Families { get; set; }
        public IList<Fee> Fees { get; set; }
        public Discount Discount { get; set; }
    }
}