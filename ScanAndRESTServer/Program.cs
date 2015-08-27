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
            Post["/Barcode"] = parameters =>
                {
                    return string.Format("{0}\r\nHello World\r\n{1}", this.BodyAsString, DateTime.Now.ToString());
                };
            Post["/Other/Barcode"] = parameters =>
                {
                    return string.Format("{0}\r\nOther World\r\n{1}", this.BodyAsString, DateTime.Now.ToString());
                };
        }

        private static Encoding BodyEncoding = new System.Text.UTF8Encoding();

        private String BodyAsString
        {
            get
            {
                try
                {
                    byte[] b = new byte[this.Request.Body.Length];
                    this.Request.Body.Read(b, 0, Convert.ToInt32(this.Request.Body.Length));
                    return BodyEncoding.GetString(b);
                }
                catch
                {
                    return "";
                }
            }
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
