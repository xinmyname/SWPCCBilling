using System;
using System.Linq;
using Nancy;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;
using SWPCCBilling.ViewModels;

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

                var family = new Family("", "", "", "", today, 1);
                var newParent = new Parent("New", "New");
                var newChild = new Child("New", "New", Child.ChildRoomPreschool1, today, now);
                newChild.Mon = newChild.Wed = true;
                family.Parents.Add(newParent);
                family.Children.Add(newChild);

                familyStore.Add(family);

                return Response.AsRedirect("/families/" + family.Id);
            };

            Get["/families/{id}"] = _ =>
            {
                var allFamilies = familyStore.LoadAll().ToList();
                var family = allFamilies.Single(f => f.Id == _.id);
                var familyEditor = new FamilyEditorViewModel
                {
                    AllFamilies = allFamilies,
                    Family = family
                };

                return View["Edit", familyEditor];
            };
        }
    }
}