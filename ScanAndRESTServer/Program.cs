using System;
using Nancy.Hosting.Self;
using System.Net.Sockets;
using System.Threading;

namespace ScanAndRESTServer
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            string HostUriAsString = string.Format("http://{0}:{1}", "localhost", "9876");
            using (NancyHost host = new NancyHost(new Uri(HostUriAsString)))
            {
                Console.WriteLine("RESTServer started on {0}", HostUriAsString);
                host.Start();
                foreach (var ipAddress in Helper.GetIpAddresses())
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                        Console.WriteLine(ipAddress.ToIPAddressString());
                Console.WriteLine("Return will terminate the program");
                while (true)
                {
                    if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Enter)
                        break;
                    Thread.Sleep(100);
                    Helper.ClipboardMainThread();
                }
            }
        }
    }
}
