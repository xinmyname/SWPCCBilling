using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;

namespace SWPCCBilling.Modules
{
    public class ReportsModule : NancyModule
    {
        public ReportsModule(FamilyStore familyStore, FeeStore feeStore, LedgerStore ledgerStore)
        {
            Get["/reports"] = _ => View["Index"];

            Get["/reports/childdays"] = _ => View["ChildDays", familyStore.LoadAll()];

            Get["/reports/returningfamilies"] = _ => View["Families", 
                familyStore.LoadAll().Where(f => f.JoinedDate.Value.Year < DateTime.Today.Year )];

            Get["/reports/newfamilies"] = _ => View["Families",
                familyStore.LoadAll().Where(f => f.JoinedDate.Value.Year == DateTime.Today.Year)];

            Get["/reports/emails"] = _ => View["Emails",
                familyStore.LoadAll().SelectMany(f => f.Parents).Where(p => !String.IsNullOrEmpty(p.Email)).Select(p => p.Email)];

            Get["/reports/ledger"] = _ =>
            {
                var families = familyStore.LoadAll().ToList();
                var fees = feeStore.LoadAll().ToList();
                var lines = ledgerStore.LoadAll()
                    .ToList()
                    .Select(l => new LedgerLineReport
                                 {
                                     Date = l.Date.ToSQLiteDate().Value.ToShortDateString(),
                                     FamilyName = families.Single(f => f.Id == l.FamilyId).FamilyName,
                                     Description = GetDescription(l, fees),
                                     UnitPrice = l.UnitPrice.ToString("C"),
                                     Quantity = l.Quantity,
                                     Amount = l.Amount.ToString("C"),
                                     Notes = l.Notes
                                 });
                return View["Ledger", lines];
            };

            Post["/reports/monthly"] = _ =>
            {
                var month = DateTime.Parse(Request.Form["month"]);
                var statements = new List<Statement>();
                decimal totalAmountDue = 0m;
                decimal totalAmoutPaid = 0m;


                foreach (var family in familyStore.LoadAll())
                {
                    var statement = new Statement(family.FamilyName);

                    statements.Add(statement);
                }


                var model = new
                {
                    Statements = statements,
                    TotalAmountDue = totalAmountDue.ToString("F2"),
                    TotalAmountPaid = totalAmoutPaid.ToString("F2")    
                };
                return View["Monthly", model];
            };
        }

        private string GetDescription(LedgerLine ledgerLine, IEnumerable<Fee> fees)
        {
            string description = String.Empty;

            if (ledgerLine.FeeId.HasValue)
            {
                Fee fee = fees.Single(f => f.Id == ledgerLine.FeeId);
                description = fee.Name;
            }

            return description;
        }
    }
}