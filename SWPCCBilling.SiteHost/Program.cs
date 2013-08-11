using System;
using System.Configuration;
using System.Diagnostics;
using Nancy.Hosting.Self;

namespace SWPCCBilling
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostUrl = ConfigurationManager.AppSettings["hostUrl"];

            var nancyHost = new NancyHost(new Uri(hostUrl));
            nancyHost.Start();

            Console.WriteLine("Nancy now listening - navigating to {0}. Press enter to stop", hostUrl);
            Process.Start(hostUrl);
            Console.ReadKey();

            nancyHost.Stop();

            Console.WriteLine("Stopped. Good bye!");
        }
    }
}
