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
        public ReportsModule(FamilyStore familyStore, FeeStore feeStore, LedgerStore ledgerStore, InvoiceStore invoiceStore, PaymentStore paymentStore)
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
                DateTime month = DateTime.Parse(Request.Form["month"]);
                var statements = new List<Statement>();
                decimal totalAmountDue = 0m;
                decimal totalAmoutPaid = 0m;
                decimal totalDonated = 0m;
                decimal totalBalance = 0m;

                Fee donationFee = feeStore.LoadDonation();

                foreach (var family in familyStore.LoadAll())
                {
                    Invoice invoice = invoiceStore.Load(month, family.Id, family.FamilyName);
                    totalAmountDue += invoice.AmountDue;

                    var statement = new Statement(family.FamilyName, invoice.AmountDue);

                    foreach (Payment payment in paymentStore.Load(month, family.Id))
                    {
                        var amount = (decimal) payment.Amount;
                        statement.AddPayment(payment.CheckNum, amount);
                        totalAmoutPaid += amount;
                    }

                    totalBalance += statement.Balance;

                    if (donationFee != null)
                    {
                        foreach (var line in ledgerStore.Load(month, family.Id, donationFee.Id))
                        {
                            var amount = (decimal) line.Amount;
                            statement.AddDonation(amount);
                            totalDonated += amount;
                        }
                    }

                    statements.Add(statement);
                }

                var model = new
                {
                    Month = month.ToString("MMMM yyyy"),
                    Statements = statements,
                    TotalAmountDue = totalAmountDue.ToString("C"),
                    TotalAmountPaid = totalAmoutPaid.ToString("C"),
                    TotalDonated = totalDonated.ToString("C"),
                    TotalBalance = totalBalance.ToString("C")
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