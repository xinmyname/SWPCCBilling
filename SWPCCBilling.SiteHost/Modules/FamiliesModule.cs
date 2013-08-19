using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
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

                var family = new Family("", "Portland", "OR", "", today, 1);
                var newParent = new Parent("", "New");
                var newChild = new Child("", "New", Child.ChildRoomPreschool1, today, now);
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

            Post["/families/{id}"] = _ =>
            {
                Family family = this.Bind();
                familyStore.Save(family);
                return Response.AsRedirect("/families");
            };

            Get["/families/{familyId}/parent/add"] = _ =>
            {
                long familyId = _.familyId;
                familyStore.AddParent(new Parent(familyId, "New", "New"));
                return Response.AsRedirect(String.Format("/families/{0}#parents", familyId));
            };

            Get["/families/{familyId}/child/add"] = _ =>
            {
                long familyId = _.familyId;
                familyStore.AddChild(new Child(familyId, "New", "New", Child.ChildRoomPreschool1, DateTime.Today, DateTime.Now));
                return Response.AsRedirect(String.Format("/families/{0}#children", familyId));
            };

            Get["/families/{familyId}/parent/{parentId}"] = _ =>
            {
                long familyId = _.familyId;
                long parentId = _.parentId;

                var allFamilies = familyStore.LoadAll().ToList();
                var family = allFamilies.Single(f => f.Id == familyId);
                var parent = family.Parents.Single(p => p.Id == parentId);
                var familyEditor = new ParentEditorViewModel
                {
                    AllFamilies = allFamilies,
                    Family = family,
                    Parent = parent
                };

                return View["ParentEdit", familyEditor];
            };

            Post["/families/{familyId}/parent/{id}"] = _ =>
            {
                long familyId = _.familyId;

                Parent parent = this.Bind();
                familyStore.SaveParent(parent);

                return Response.AsRedirect(String.Format("/families/{0}#parents", familyId));
            };

        }
    }
}