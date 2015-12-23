using System;
using System.Windows.Forms;
using Nancy;
using Nancy.Hosting.Self;

namespace ScanAndRESTClipboard
{
    public class Module : NancyModule
    {
        public Module()
            : base()
        {
            Before += nancyContext =>
            {
                //do stuff with the context.
                return null; //to not affect the the response return response otherwise
            };

            Get["/Scan"] = parameters =>
            {
                var barcode = (string)Request.Query.Barcode;

                    Gtk.Clipboard clipboard = Gtk.Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false));

                    clipboard.Text = "Hello World";

                Clipboard.SetText(barcode);
                return string.Format("{0}\r\nPut to Clipboard\r\n{1}", barcode, DateTime.Now.ToString());


                
            };

            After += nancyContext =>
            {
                //do stuff with the context.             
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
