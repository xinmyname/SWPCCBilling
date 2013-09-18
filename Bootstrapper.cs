using System;
using System.Configuration;
using System.Diagnostics;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Models;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Hosting.Self;
using Nancy.ViewEngines.Razor;
using log4net;
using Autofac;
using Nancy.Conventions;
using Nancy.ViewEngines;
using Nancy.Embedded.Conventions;

namespace SWPCCBilling
{
	class Bootstrapper : AutofacNancyBootstrapper
	{
	    private readonly ILog _log;
	    private readonly string _hostUrl;
	    private readonly NancyHost _host;


	    public static void Main(string[] args)
	    {
	        var log = LogManager.GetLogger("SWPCCBilling");

	        try
	        {
	            var bootstrapper = new Bootstrapper(
	                log,
	                ConfigurationManager.AppSettings["HostUrl"]);

                bootstrapper.Run();
	        }
	        catch (Exception ex)
	        {
	            log.Fatal(ex);
	        }
		}

	    public Bootstrapper(ILog log, string hostUrl)
	    {
	        _log = log;
	        _hostUrl = hostUrl;

            var hostConfig = new HostConfiguration
            {
                UrlReservations = {CreateAutomatically = true}
            };

	        _host = new NancyHost(this, hostConfig, new Uri(_hostUrl));
	    }

        public void Run()
        {
            _host.Start();

            _log.Info("SWPCCBilling host now listening");
            _log.InfoFormat("Navigating to {0}", _hostUrl);
            _log.Info("");
            _log.Info("Press Enter to stop");

            Process.Start(_hostUrl);
            Console.ReadKey();

            _host.Stop();

            _log.Info("Stopped. Goodbye!");
        }

	    protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
	    {
	        base.ConfigureApplicationContainer(existingContainer);

	        var builder = new ContainerBuilder();

	        builder.RegisterInstance(_log).As<ILog>();
	        builder.RegisterAssemblyTypes(GetType().Assembly);
			builder.RegisterType<DefaultViewRenderer>();

	        builder.Update(existingContainer.ComponentRegistry);
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

	    protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
	    {
	        base.ApplicationStartup(container, pipelines);

	        var settingsStore = container.Resolve<SettingsStore>();
	        var resStore = container.Resolve<ResourceStore>();

	        if (settingsStore.Load() == null)
	        {
                var settings = new Settings
                {
                    DatabasePath = DocumentPath.For("SWPCCBilling", "s3db")
                };

                resStore.Deploy("SWPCCBilling.empty.s3db", settings.DatabasePath);

	            settingsStore.Save(settings);
	        }
	    }

		protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, Nancy.NancyContext context)
		{
			pipelines.OnError.AddItemToEndOfPipeline((z,a) => 
			{
				_log.Error("Unhandled error on request: " + context.Request.Url + " : " + a.Message, a);
				return null;
			});

			base.RequestStartup(container, pipelines, context);
		}
	}
}
