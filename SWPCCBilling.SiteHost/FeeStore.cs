using System;
using System.Collections.Generic;
using SWPCCBilling.Models;

namespace SWPCCBilling
{
    public class FeeStore
    {
        public IEnumerable<Fee> LoadAll()
        {
            return new List<Fee>
            {
                new Fee(1, "Tuition", "F", 103.00m),
                new Fee(2, "Insurance (Full)", "F", 85.00m)
            };
        }
    }
}