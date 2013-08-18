using System;
using Nancy;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;

namespace SWPCCBilling.Modules
{
    public class FamiliesModule : NancyModule
    {
        public FamiliesModule(FamilyStore familyStore)
        {
            Get["/families"] = _ => View["Index", familyStore.LoadAll()];

            Get["/families/add"] = _ =>
            {
                DateTime now = DateTime.Now;
                DateTime today = DateTime.Today;

                var family = new Family("","","","", today, 1);
                family.Parents.Add(new Parent("","New Family"));
                family.Children.Add(new Child("New Child", "", Child.ChildRoomPreschool1, today, now));

                familyStore.Add(family);

                return Response.AsRedirect("/families");
                //return Response.AsRedirect("/families/" + family.Id);
            };
        }
    }
}