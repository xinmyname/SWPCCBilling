using Nancy;
using Nancy.Conventions;

namespace SWPCCBilling.SiteHost
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(
                    EmbeddedStaticContentConventionBuilder.AddDirectory(
                    "/lib",
                    GetType().Assembly,
                    "SiteHost.bower_components"));

            conventions.StaticContentsConventions.Add(
                    EmbeddedStaticContentConventionBuilder.AddDirectory(
                    "/",
                    GetType().Assembly,
                    "SiteHost.Content"));
        }
    }
}
