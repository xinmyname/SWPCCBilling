using Nancy;
using Nancy.Conventions;

namespace SWPCCBilling
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
                    "bower_components"));

            conventions.StaticContentsConventions.Add(
                    EmbeddedStaticContentConventionBuilder.AddDirectory(
                    "/",
                    GetType().Assembly,
                    "Content"));
        }
    }
}
