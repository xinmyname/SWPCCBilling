namespace SWPCCBilling.Infrastructure
{
    public class PaymentStore
    {
        private readonly DatabaseFactory _dbFactory;

        public PaymentStore(DatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
    }
}