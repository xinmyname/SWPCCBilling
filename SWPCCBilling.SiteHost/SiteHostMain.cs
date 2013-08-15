using System;
using System.Diagnostics;
using System.Linq;
using Nancy.Hosting.Self;
using log4net;
using SWPCCBilling.Properties;

namespace SWPCCBilling
{
    class SiteHostMain
    {
        private readonly ILog _log;
        private readonly NancyHost _host;
        private readonly string _hostUrl;

        static void Main(string[] args)
        {
            var log = LogManager.GetLogger("SWPCCBilling");

            switch (args.FirstOrDefault())
            {
                case "/?":
                case "/h":
                case "-?":
                case "-h":
                case "--help":
                    ShowHelp();
                    return;
                case "/res":
                    ListEmbeddedResources();
                    return;
            }

            try
            {
                string hostUrl = Settings.Default.HostUrl;

                new SiteHostMain(log, new NancyHost(new Uri(hostUrl)), hostUrl)
                    .Run();
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
            }
        }

        public static void ShowHelp()
        {
            Console.WriteLine("SWPCC Billing Site Host");
            Console.WriteLine("-----------------------");
            Console.WriteLine("");
            Console.WriteLine("Windows Usage: SWPCCBilling.exe [options]");
            Console.WriteLine("OSX Usage:     mono SWPCCBilling.exe [options]");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("     /?      Show help");
            Console.WriteLine("     /res    List embedded resources");
        }

        public static void ListEmbeddedResources()
        {
            string[] resources = typeof (SiteHostMain).Assembly.GetManifestResourceNames();

            foreach (string resource in resources)
                Console.WriteLine(resource);
        }

        public SiteHostMain(ILog log, NancyHost host, string hostUrl)
        {
            _log = log;
            _host = host;
            _hostUrl = hostUrl;
        }

        public void Run()
        {
            _host.Start();

            _log.Info("SWPCC Billing Site Host now listening");
            _log.InfoFormat("Navigating to {0}", _hostUrl);
            _log.Info("");
            _log.Info("Press Enter to stop.");

            Process.Start(_hostUrl);
            Console.ReadKey();

            _host.Stop();

            _log.Info("Stopped. Good bye!");
        }
    }
}
