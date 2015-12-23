using System;
using Nancy.Hosting.Self;

namespace ScanAndRESTServer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string HostUriAsString = string.Format("http://{0}:{1}", "localhost", "9876");
            using (NancyHost host = new NancyHost(new Uri(HostUriAsString)))
            {
                host.Start();
                Console.WriteLine("RESTServer started on {0}, Return will end the program", HostUriAsString);
                Console.ReadLine();
            }
        }


    }
}
