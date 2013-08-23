using System;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class LedgerLineFactory
    {
        public LedgerLine CalculateCharge(ChargeRequest chargeRequest, Fee fee, Family family)
        {
            decimal amount = 0.0m;

            switch (fee.Type)
            {
                case Fee.FeeTypeFixed:
                    if (fee.Amount == null)
                        throw new ArgumentException("Fixed fee must have an amount!");

                    amount = (decimal)fee.Amount.Value;
                    break;

                case Fee.FeeTypeVarying:
                    amount = chargeRequest.ChargeAmount;
                    break;

                case Fee.FeeTypePerChild:
                    if (fee.Amount == null)
                        throw new ArgumentException("Fixed fee must have an amount!");

                    amount = ((decimal)fee.Amount.Value) * family.NumChildren;
                    break;

                case Fee.FeeTypePerChildDay:
                    if (fee.Amount == null)
                        throw new ArgumentException("Fixed fee must have an amount!");

                    amount = ((decimal)fee.Amount.Value) * family.ChildDays;
                    break;

                case Fee.FeeTypePerMinute:
                    if (fee.Amount == null)
                        throw new ArgumentException("Fixed fee must have an amount!");

                    amount = ((decimal)fee.Amount.Value) * chargeRequest.ChargeMinutes;
                    break;
            }

            return CreateCharge(family.Id, fee.Id, chargeRequest.ChargeDate, amount, chargeRequest.ChargeNotes);
        }

        public LedgerLine CreateCharge(long familyId, long feeId, DateTime date, decimal amount, string notes)
        {
            return new LedgerLine
            {
                FamilyId = familyId,
                Date = date.ToSQLiteDateTime(),
                FeeId = feeId,
                Amount = (double)amount,
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
                Amount = (double)amount,
                Notes = notes
            };
        }
         
    }
}