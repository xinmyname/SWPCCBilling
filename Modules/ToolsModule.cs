using System.IO;
using Nancy;
using SWPCCBilling.Infrastructure;
using log4net;
using SWPCCBilling.Models;

namespace SWPCCBilling.Modules
{
    public class ToolsModule : NancyModule
    {
        public ToolsModule(ILog log, SettingsStore settingsStore)
        {
			Settings settings = settingsStore.Load();

            Get["/tools"] = _ =>
            {
                var model = new
                {
                    settings.EmailServer,
					settings.EmailPort,
					settings.EmailFrom,
					settings.EmailSSL
                };
                return View["Index", model];
            }; 

            Post["/tools/new"] = _ =>
            {
                string dbPath = DocumentPath.For(Request.Form.Name, "s3db");

                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                Stream srcStream = GetType().Assembly.GetManifestResourceStream("SWPCCBilling.Resources.SWPCCBilling.empty.s3db");
                Stream dstStream = new FileStream(dbPath, FileMode.CreateNew);

                srcStream.CopyTo(dstStream);

                srcStream.Close();
                dstStream.Close();

                settings.DatabasePath = dbPath;
                settingsStore.Save(settings);

                log.InfoFormat("Started new school year file: {0}", dbPath);

                return Response.AsRedirect("/");
            };

            Post["/tools/email"] = _ =>
            {
                settings.EmailServer = Request.Form.Server;
				settings.EmailPort = Request.Form.Port;
				settings.EmailFrom = Request.Form.From;

                if (Request.Form.UseSSL.HasValue)
					settings.EmailSSL = Request.Form.UseSSL == "on";
                else
					settings.EmailSSL = false;

                settingsStore.Save(settings);

                return Response.AsRedirect("/");
            };
        }
    }
}