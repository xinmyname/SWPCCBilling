using log4net;

namespace SWPCCBilling.Modules
{
    public class HomeModule : Nancy.NancyModule
    {
        private readonly ILog _log;

        public HomeModule(ILog log)
        {
            _log = log;

            Get["/"] = _ =>
            {
                _log.Info("Index!");
                return View["Index"];
            };
        }
    }
}