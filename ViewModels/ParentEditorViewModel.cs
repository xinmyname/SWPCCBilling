using System.Collections.Generic;
using SWPCCBilling.Models;

namespace SWPCCBilling.ViewModels
{
    public class ParentEditorViewModel
    {
        public IList<Family> AllFamilies { get; set; }
        public Family Family { get; set; }
        public Parent Parent { get; set; }
    }
}