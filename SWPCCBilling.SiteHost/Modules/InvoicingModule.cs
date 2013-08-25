using Nancy;
using Nancy.ViewEngines;
using SWPCCBilling.Infrastructure;

namespace SWPCCBilling.Modules
{
    public class InvoicingModule : NancyModule
    {
        public InvoicingModule(DefaultViewRenderer viewRenderer, FamilyStore familyStore)
        {
            Get["/invoicing"] = _ =>
            {
                return View["Index"];
            };

            Get["/invoicing/test"] = _ =>
            {
                var model = new { FamilyName = "Test Family" };
                Response response = viewRenderer.RenderView(Context, "InvoiceTemplate", model);
                return View["InvoiceTemplate", model];
            };
        }
    }
}