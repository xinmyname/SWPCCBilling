using System.Collections.Generic;
using SWPCCBilling.Models;

namespace SWPCCBilling.ViewModels
{
    public class FamilyEditorViewModel
    {
        public IList<Family> AllFamilies { get; set; }
        public Family Family { get; set; }
    }
}