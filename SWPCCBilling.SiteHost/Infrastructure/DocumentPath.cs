using System;
using System.IO;

namespace SWPCCBilling.Infrastructure
{
    public class DocumentPath
    {
        public static string For(string name, string extension)
        {
            return String.Format("{1}{0}SWPCCBilling{0}{2}.{3}",
                Path.DirectorySeparatorChar,
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                name,
                extension);
        }
    }
}