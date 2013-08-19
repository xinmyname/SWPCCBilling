using System.Collections.Generic;
using SWPCCBilling.Models;

namespace SWPCCBilling.ViewModels
{
    public class ChildEditorViewModel
    {
        public IList<Family> AllFamilies { get; set; }
        public Family Family { get; set; }
        public Child Child { get; set; }
    }
}