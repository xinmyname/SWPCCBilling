using System;
using System.Diagnostics;
using System.Linq;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Hosting.Self;
using Nancy.ViewEngines;
using Ninject;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Properties;
using log4net;

namespace SWPCCBilling
{
    public class Bootstrapper : NinjectNancyBootstrapper
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

                new Bootstrapper(log, hostUrl)
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
            string[] resources = typeof (Bootstrapper).Assembly.GetManifestResourceNames();

            foreach (string resource in resources)
                Console.WriteLine(resource);
        }

        public Bootstrapper(ILog log, string hostUrl)
        {
            _log = log;
            _host = new NancyHost(this, new Uri(hostUrl));
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

        protected override void ConfigureApplicationContainer(IKernel kernel)
        {
            base.ConfigureApplicationContainer(kernel);

            kernel.Bind<ILog>().ToConstant(_log);
        }

        protected override void ConfigureRequestContainer(IKernel kernel, NancyContext context)
        {
            base.ConfigureRequestContainer(kernel, context);
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(
                    EmbeddedStaticContentConventionBuilder.AddDirectory(
                    "/lib",
                    GetType().Assembly,
                    "bower_components"));

            conventions.StaticContentsConventions.Add(
                    EmbeddedStaticContentConventionBuilder.AddDirectory(
                    "/css",
                    GetType().Assembly,
                    "css"));

            ResourceViewLocationProvider
                .RootNamespaces
                .Add(GetType().Assembly, "SWPCCBilling.Views");
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get { return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder); }
        }

        void OnConfigurationBuilder(NancyInternalConfiguration x)
        {
            x.ViewLocationProvider = typeof(ResourceViewLocationProvider);
        }
    }
}
