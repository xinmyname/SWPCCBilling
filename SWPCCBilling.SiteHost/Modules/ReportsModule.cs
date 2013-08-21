using Nancy;
using SWPCCBilling.Infrastructure;

namespace SWPCCBilling.Modules
{
    public class ReportsModule : NancyModule
    {
        public ReportsModule(FamilyStore familyStore)
        {
            Get["/reports"] = _ => View["Index"];
            Get["/reports/childdays"] = _ => View["ChildDays", familyStore.LoadAll()];
        }
    }
}