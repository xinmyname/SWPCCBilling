﻿using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;

namespace SWPCCBilling.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(
            FamilyStore familyStore, 
            FeeStore feeStore, 
            LedgerStore ledgerStore, 
            LedgerLineFactoryFactory lineFactoryFactory, 
            PaymentStore paymentStore)
        {
            Get["/"] = _ =>
            {
                var nextMonth = DateTime.Now.AddDays(-1).AddMonths(1);
                nextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);

                var model = new
                {
                    Families = familyStore.LoadAll().OrderBy(f => f.FamilyName).ToList(),
                    Fees = feeStore.LoadAll().ToList(),
                    NextMonth = nextMonth.ToShortDateString(),
                    Today = DateTime.Now.ToShortDateString()
                };

                return View["Index", model];
            };

            Post["/home/charge"] = _ =>
            {
                var chargeRequest = this.Bind<ChargeRequest>();
                var receipt = new ChargeReceipt();
                Fee fee = feeStore.Load(chargeRequest.FeeId);
                LedgerLineFactory lineFactory = lineFactoryFactory.Create();

                foreach (long familyId in chargeRequest.GetFamilyIds())
                {
                    Family family = familyStore.Load(familyId);

                    foreach (LedgerLine line in lineFactory.CalculateCharges(chargeRequest, fee, family))
                    {
                        ledgerStore.Add(line);
                        receipt.Amount += (decimal) line.Amount;
                    }

                    receipt.NumFamilies++;
                }

                return Response.AsJson(receipt);
            };

            Post["/home/payment"] = _ =>
            {
                var paymentRequest = this.Bind<PaymentRequest>();
                var receipt = new PaymentReceipt();
                Family family = familyStore.Load(paymentRequest.FamilyId);

                var payment = new Payment
                {
                    FamilyId = family.Id,
                    CheckNum = paymentRequest.CheckNum,
                    Amount = (double) paymentRequest.Amount,
                    Received = paymentRequest.Date.ToSQLiteDate()   
                };

                paymentStore.Add(payment);

                LedgerLineFactory lineFactory = lineFactoryFactory.Create();

                LedgerLine line = lineFactory.CreatePayment(family.Id, payment.Id, paymentRequest.Date, paymentRequest.Amount,
                    paymentRequest.Notes);

                ledgerStore.Add(line);

                receipt.FamilyName = family.FamilyName;
                receipt.Amount = paymentRequest.Amount;

                return Response.AsJson(receipt);
            };
        }

    }
}