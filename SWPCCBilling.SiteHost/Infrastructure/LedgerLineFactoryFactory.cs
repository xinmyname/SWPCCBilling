namespace SWPCCBilling.Infrastructure
{
    public class LedgerLineFactoryFactory
    {
        private readonly DiscountStore _discountStore;

        public LedgerLineFactoryFactory(DiscountStore discountStore)
        {
            _discountStore = discountStore;
        }

        public LedgerLineFactory Create()
        {
            return new LedgerLineFactory(_discountStore.LoadAll());
        }
    }
}