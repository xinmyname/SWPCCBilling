using System;
using System.Data;
using Community.CsharpSqlite.SQLiteClient;
using Nancy;
using Nancy.Conventions;
using Nancy.TinyIoc;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Properties;

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

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
        }
    }
}
