using System;
using System.Collections.Generic;
using System.Linq;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class LedgerLineFactory
    {
        private readonly IList<Discount> _discounts;
        private readonly long _financialAidFeeId;

        public LedgerLineFactory(IEnumerable<Discount> discounts, IEnumerable<Fee> fees)
        {
            _discounts = new List<Discount>(discounts);
            Fee financialAidFee = fees.SingleOrDefault(f => f.Name == "Financial Aid");
            _financialAidFeeId = financialAidFee != null
                ? financialAidFee.Id
                : -1;
        }

        public IEnumerable<LedgerLine> CalculateCharges(ChargeRequest chargeRequest, Fee fee, Family family)
        {
            decimal unitPrice = 0.0m;
            long quantity = 0;

            switch (fee.Type)
            {
                case Fee.FeeTypeFixed:
                    if (fee.Amount == null)
                        throw new ArgumentException("Fixed fee must have an amount!");

                    unitPrice = (decimal)fee.Amount.Value;
                    quantity = 1;
                    break;

                case Fee.FeeTypeVarying:
                    unitPrice = chargeRequest.UnitPrice;
                    quantity = chargeRequest.Quantity;
                    break;

                case Fee.FeeTypePerChild:
                    if (fee.Amount == null)
                        throw new ArgumentException("Fixed fee must have an amount!");

                    unitPrice = (decimal) fee.Amount.Value;
                    quantity = family.NumChildren;
                    
                    break;

                case Fee.FeeTypePerChildDay:
                    if (fee.Amount == null)
                        throw new ArgumentException("Fixed fee must have an amount!");

                    unitPrice = (decimal) fee.Amount.Value;
                    quantity = family.ChildDays;
                    break;

                case Fee.FeeTypePerMinute:
                    if (fee.Amount == null)
                        throw new ArgumentException("Fixed fee must have an amount!");

                    unitPrice = (decimal) fee.Amount.Value;
                    quantity = chargeRequest.ChargeMinutes;
                    break;
            }

            return CreateCharges(family.Id, fee.Id, chargeRequest.ChargeDate, unitPrice, quantity, chargeRequest.ChargeNotes);
        }

        public IEnumerable<LedgerLine> CreateCharges(long familyId, long feeId, DateTime date, decimal unitPrice, long quantity, string notes)
        {
            var discount = _discounts.SingleOrDefault(d => d.FamilyId == familyId && d.FeeId == feeId);

            if (discount != null && !discount.IsFinancialAid)
                unitPrice -= unitPrice * ((decimal)discount.Percent) / 100m;

            yield return new LedgerLine
            {
                FamilyId = familyId,
                Date = date.ToSQLiteDateTime(),
                FeeId = feeId,
                UnitPrice = (double)unitPrice,
                Quantity = quantity,
                Amount = (double)(unitPrice * quantity),
                Notes = notes
            };

            if (discount != null && discount.IsFinancialAid)
            {
                decimal financialAidAmount = unitPrice*((decimal) discount.Percent)/-100m;

                // Credit back discounted amount
                yield return new LedgerLine
                {
                    FamilyId = familyId,
                    Date = date.ToSQLiteDateTime(),
                    FeeId = _financialAidFeeId,
                    UnitPrice = (double)financialAidAmount,
                    Quantity = quantity,
                    Amount = (double)(financialAidAmount * quantity),
                    Notes = notes
                };
            }
        }

        public LedgerLine CreatePayment(long familyId, long paymentId, DateTime date, decimal amount, string notes)
        {
            return new LedgerLine
            {
                FamilyId = familyId,
                Date = date.ToSQLiteDateTime(),
                PaymentId = paymentId,
                UnitPrice = 0,
                Quantity = 0,
                Amount = (double)(amount * -1.0m),
                Notes = notes
            };
        }
         
    }
}