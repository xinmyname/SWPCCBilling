using System;
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
    public class ReportsModule : NancyModule
    {
        private readonly FamilyStore _familyStore;
        private readonly FeeStore _feeStore;
        private readonly LedgerStore _ledgerStore;
        private readonly InvoiceStore _invoiceStore;
        private readonly PaymentStore _paymentStore;

        public ReportsModule(FamilyStore familyStore, FeeStore feeStore, LedgerStore ledgerStore, InvoiceStore invoiceStore, PaymentStore paymentStore)
        {
            _familyStore = familyStore;
            _feeStore = feeStore;
            _ledgerStore = ledgerStore;
            _invoiceStore = invoiceStore;
            _paymentStore = paymentStore;

            Get["/reports"] = _ =>
            {
                var model = new
                {
                    DepositDates = paymentStore.LoadDepositDates()
                };

                return View["Index", model];
            };

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
                                     FamilyName = GetFamilyName(l.FamilyId, families),
                                     Description = GetDescription(l, fees),
                                     UnitPrice = l.UnitPrice.ToString("C"),
                                     Quantity = l.Quantity,
                                     Amount = l.Amount.ToString("C"),
                                     Notes = l.Notes
                                 });
                return View["Ledger", lines];
            };

            Post["/reports/unpaid"] = _ =>
            {
                DateTime month = DateTime.Parse(Request.Form["month"]);

                var model = GetStatements(month)
                    .Where(s => s.Balance < 0m)
                    .Select(s => familyStore.Load(s.FamilyId))
                    .SelectMany(f => f.Parents)
                    .Where(p => !String.IsNullOrEmpty(p.Email))
                    .Select(p => p.Email);

                return View["Emails", model];
            };

            Post["/reports/monthly"] = _ =>
            {
                DateTime month = DateTime.Parse(Request.Form["month"]);
                decimal totalAmountDue = 0m;
                decimal totalAmoutPaid = 0m;
                decimal totalDonated = 0m;
                decimal totalBalance = 0m;

                IList<Statement> statements = GetStatements(month)
                    .OrderBy(s => s.DepositDate)
                    .ThenBy(s => s.FamilyName)
                    .ToList();

                foreach (var statement in statements)
                {
                    totalAmountDue += statement.AmountDue;
                    totalAmoutPaid += statement.AmountPaid;
                    totalDonated += statement.Donated;
                    totalBalance += statement.Balance;
                }

                IList<DepositViewModel> deposits = GetDeposits(month).ToList();

                var model = new
                {
                    Month = month.ToString("MMMM yyyy"),
                    Statements = statements,
                    TotalAmountDue = totalAmountDue.ToString("C"),
                    TotalAmountPaid = totalAmoutPaid.ToString("C"),
                    TotalDonated = totalDonated.ToString("C"),
                    TotalBalance = totalBalance.ToString("C"),
                    Deposits = deposits
                };
                return View["Monthly", model];
            };

            Post["/reports/deposit"] = _ =>
            {
                DateTime depositDate = Request.Form["DepositDate"];
                var monthStart = new DateTime(depositDate.Year, depositDate.Month, 1);
                var fees = _feeStore.LoadAll().ToLookup(f => f.Id);
                var families = _familyStore.LoadAll().ToLookup(f => f.Id);

                var model = _ledgerStore.Load(monthStart, depositDate)
                    .Select(ll =>
                        new PaymentDepositViewModel
                        {
                            FamilyId = ll.FamilyId,
                            FamilyName = families[ll.FamilyId].Single().FamilyName,
                            Date = ll.Date.ToSQLiteDate(),
                            DateText = ll.Date.ToSQLiteDate().HasValue
                                ? ll.Date.ToSQLiteDate().Value.ToShortDateString()
                                : "",
                            FeeId = ll.FeeId,
                            FeeName = ll.FeeId.HasValue 
                                ? fees[ll.FeeId.Value].Single().Name
                                : "",
                            Amount = (decimal)ll.Amount,
                            AmountText = ll.Amount.ToString("C"),
                            Notes = ll.Notes
                        });

                return View["PaymentsDeposit", model];
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

        private IEnumerable<Statement> GetStatements(DateTime month)
        {
            Fee donationFee = _feeStore.LoadDonation();

            var statements = new List<Statement>();

            foreach (var family in _familyStore.LoadAll().OrderBy(f => f.FamilyName))
            {
                Invoice invoice = _invoiceStore.Load(month, family.Id, family.FamilyName);

                if (invoice == null)
                    continue;

                var statement = new Statement(family.Id, family.FamilyName, invoice.AmountDue);

                foreach (Payment payment in _paymentStore.Load(month, family.Id))
                    statement.AddPayment(payment.CheckNum, (decimal)payment.Amount, payment.Deposited.ToSQLiteDateTime());

                if (donationFee != null)
                {
                    foreach (var line in _ledgerStore.Load(month, family.Id, donationFee.Id))
                    {
                        var amount = (decimal)line.Amount;
                        statement.AddDonation(amount);
                    }
                }

                statements.Add(statement);
            }

            return statements;
        }

        private IEnumerable<DepositViewModel> GetDeposits(DateTime month)
        {
            int n = 0;
            return
                from p in _paymentStore.Load(month)
                where p.Deposited != null
                group (decimal) p.Amount by p.Deposited.ToSQLiteDateTime()
                into gr
                select new DepositViewModel(++n, gr.Key, gr.Sum());
        }

        private string GetFamilyName(long familyId, IEnumerable<Family> families)
        {
            Family family = families.SingleOrDefault(f => f.Id == familyId);
            if (family == null)
                return String.Format("(Departed Family {0})", familyId);
            return family.FamilyName;
        }
    }
}