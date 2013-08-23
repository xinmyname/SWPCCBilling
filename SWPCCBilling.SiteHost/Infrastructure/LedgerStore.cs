using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class LedgerStore
    {
        private readonly DatabaseFactory _dbFactory;

        public LedgerStore(DatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public LedgerLine Add(LedgerLine line)
        {
            throw new NotImplementedException();
        }
    }
}
