using System;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class LedgerLineFactory
    {
        public LedgerLine CalculateCharge(ChargeRequest chargeRequest, Fee fee, Family family)
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

            return CreateCharge(family.Id, fee.Id, chargeRequest.ChargeDate, unitPrice, quantity, chargeRequest.ChargeNotes);
        }

        public LedgerLine CreateCharge(long familyId, long feeId, DateTime date, decimal unitPrice, long quantity, string notes)
        {
            return new LedgerLine
            {
                FamilyId = familyId,
                Date = date.ToSQLiteDateTime(),
                FeeId = feeId,
                UnitPrice = (double)unitPrice,
                Quantity = quantity,
                Amount = (double)(unitPrice * quantity),
                Notes = notes
            };
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