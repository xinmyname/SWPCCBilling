using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;
using SWPCCBilling.ViewModels;

namespace SWPCCBilling.Modules
{
    public class DiscountsModule : NancyModule
    {
        public DiscountsModule(DiscountStore discountStore, FamilyStore familyStore, FeeStore feeStore)
        {
            Get["/discounts"] = _ => View["Index", discountStore.LoadAllLinks(familyStore, feeStore)];

            Get["/discounts/add"] = _ =>
            {
                var editor = new DiscountViewEditorModel
                {
                    AllDiscounts = discountStore.LoadAllLinks(familyStore, feeStore).ToList(),
                    Families = familyStore.LoadAllShallow().ToList(),
                    Fees = feeStore.LoadAll().ToList(),
                    Discount = new Discount(),
                };

                return View["Edit", editor];
            };

            Get["/discounts/{id}"] = _ =>
            {
                var allDiscounts = discountStore.LoadAll();
                var discount = allDiscounts.Single(d => d.Id == _.id);
                var editor = new DiscountViewEditorModel
                {
                    AllDiscounts = discountStore.LoadAllLinks(familyStore, feeStore).ToList(),
                    Families = familyStore.LoadAllShallow().ToList(),
                    Fees = feeStore.LoadAll().ToList(),
                    Discount = discount,
                };

                return View["Edit", editor];
            };

            Post["/discounts/{id}"] = _ =>
            {
                Discount discount = this.Bind();
                discountStore.Save(discount);
                return Response.AsRedirect("/discounts");
            };
        }
    }
}