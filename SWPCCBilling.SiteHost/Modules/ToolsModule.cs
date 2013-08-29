using System.IO;
using Nancy;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Properties;
using log4net;

namespace SWPCCBilling.Modules
{
    public class ToolsModule : NancyModule
    {
        public ToolsModule(ILog log)
        {
            Get["/tools"] = _ =>
            {
                var model = new
                {
                    Settings.Default.EmailServer,
                    Settings.Default.EmailPort,
                    Settings.Default.EmailFrom,
                    Settings.Default.EmailSSL
                };
                return View["Index", model];
            }; 

            Post["/tools/new"] = _ =>
            {
                string dbPath = DocumentPath.For(Request.Form.Name, "s3db");

                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                Stream srcStream = GetType().Assembly.GetManifestResourceStream("SWPCCBilling.Resources.SWPCCBilling.empty.sqlite.s3db");
                Stream dstStream = new FileStream(dbPath, FileMode.CreateNew);

                srcStream.CopyTo(dstStream);

                srcStream.Close();
                dstStream.Close();

                Settings.Default.DatabasePath = dbPath;
                Settings.Default.Save();

                log.InfoFormat("Started new school year file: {0}", dbPath);

                return Response.AsRedirect("/");
            };

            Post["/tools/email"] = _ =>
            {
                Settings.Default.EmailServer = Request.Form.Server;
                Settings.Default.EmailPort = Request.Form.Port;
                Settings.Default.EmailFrom = Request.Form.From;

                if (Request.Form.UseSSL.HasValue)
                    Settings.Default.EmailSSL = Request.Form.UseSSL == "on";
                else
                    Settings.Default.EmailSSL = false;

                Settings.Default.Save();

                return Response.AsRedirect("/");
            };
        }
    }
}