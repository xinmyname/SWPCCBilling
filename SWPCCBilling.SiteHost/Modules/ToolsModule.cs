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
            Get["/tools"] = _ => View["Index"]; 

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
        }
    }
}