using System;
using Nancy;
using System.Text;
using Nancy.Hosting.Self;

namespace ScanAndRESTServer
{
    public class Module : NancyModule
    {
        public Module()
            : base()
        {
            Get["/Scan"] = parameters =>
            {
                return string.Format("{0}\r\nHello World\r\n{1}", (string)Request.Query.Barcode, DateTime.Now.ToString());
            };
        }
    }

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
