namespace SWPCCBilling.Infrastructure
{
    public class LedgerLineFactoryFactory
    {
        private readonly DiscountStore _discountStore;
        private readonly FeeStore _feeStore;

        public LedgerLineFactoryFactory(DiscountStore discountStore, FeeStore feeStore)
        {
            _discountStore = discountStore;
            _feeStore = feeStore;
        }

        public LedgerLineFactory Create()
        {
            return new LedgerLineFactory(_discountStore.LoadAll(), _feeStore.LoadAll());
        }
    }
}