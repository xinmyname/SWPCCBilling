﻿using Nancy;

namespace SWPCCBilling.SiteHost
{
    public class FeeModule : NancyModule
    {
        private readonly FeeStore _feeStore;

        public FeeModule(FeeStore feeStore)
        {
            _feeStore = feeStore;
            Get["/fees"] = _ => Response.AsJson(_feeStore.LoadAll());
        }
    }
}