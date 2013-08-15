using System;
using System.IO;
using Nancy;
using SWPCCBilling.Properties;

namespace SWPCCBilling
{
    public class ToolsModule : NancyModule
    {
        public ToolsModule()
        {
            Post["/tools/new"] = _ =>
            {
                string dbPath = GetDocumentPath(Request.Form.Name, "s3db");

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

        private string GetDocumentPath(string name, string extension)
        {
            return String.Format("{1}{0}SWPCCBilling{0}{2}.{3}",
                Path.DirectorySeparatorChar,
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                name,
                extension);
        }
    }
}