using System.IO;
using Nancy;
using SWPCCBilling.Infrastructure;
using SWPCCBilling.Properties;

namespace SWPCCBilling.Modules
{
    public class ToolsModule : NancyModule
    {
        public ToolsModule()
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

                return Response.AsRedirect(Settings.Default.HostUrl);
            };
        }
    }
}