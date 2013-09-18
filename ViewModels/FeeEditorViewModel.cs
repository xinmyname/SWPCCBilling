using System.Collections.Generic;
using SWPCCBilling.Models;

namespace SWPCCBilling.ViewModels
{
    public class FeeEditorViewModel
    {
        public IList<Fee> AllFees { get; set; }
        public Fee Fee { get; set; }
    }
}
