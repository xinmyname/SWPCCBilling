using System;
using System.Diagnostics;
using Nancy.Hosting.Self;

namespace SWPCCBilling
{
    class Program
    {
        static void Main(string[] args)
        {
            const string hostUrl = "http://localhost:8733/Design_Time_Addresses/SWPCCBilling/";

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
