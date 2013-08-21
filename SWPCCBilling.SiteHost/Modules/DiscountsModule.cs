using System.Collections;
using System.Collections.Generic;
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
            Get["/discounts"] = _ =>
            {
                IList<Family> families = familyStore.LoadAll().ToList();
                IList<Fee> fees = feeStore.LoadAll().ToList();

                return View["Index", discountStore.LoadAllLinks(families, fees)];
            };

            Get["/discounts/add"] = _ =>
            {
                IList<Family> families = familyStore.LoadAll().ToList();
                IList<Fee> fees = feeStore.LoadAll().ToList();

                var editor = new DiscountViewEditorModel
                {
                    AllDiscounts = discountStore.LoadAllLinks(families, fees).ToList(),
                    Families = families,
                    Fees = fees,
                    Discount = new Discount{Id = -1},
                };

                return View["Edit", editor];
            };

            Get["/discounts/{id}"] = _ =>
            {
                IList<Family> families = familyStore.LoadAll().ToList();
                IList<Fee> fees = feeStore.LoadAll().ToList();

                var allDiscounts = discountStore.LoadAll();
                var discount = allDiscounts.Single(d => d.Id == _.id);

                var editor = new DiscountViewEditorModel
                {
                    AllDiscounts = discountStore.LoadAllLinks(families, fees).ToList(),
                    Families = families,
                    Fees = fees,
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