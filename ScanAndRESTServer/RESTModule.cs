using System;
using Nancy;

namespace ScanAndRESTServer
{
    public class Module : NancyModule
    {
        public Module()
            : base()
        {
            Before += nancyContext =>
                {
                    return null; 
                };
            Get["/Scan"] = parameters =>
                {
                    Console.WriteLine ("a");
                    var barcode = (string)Request.Query.Barcode;
                    var datetime= DateTime.Now.ToString();
                    Console.WriteLine ("Scan {0} barcode={1}", datetime, barcode);
                    return string.Format("{0}\r\nHello World\r\n{1}", barcode, datetime);
                };

            Get["/ScanToClipboard"] = parameters =>
                {
                    Console.WriteLine ("b");
                    var barcode = (string)Request.Query.Barcode;
                    var datetime= DateTime.Now.ToString();
                    Console.WriteLine ("ScanToClipboard {0} barcode={1}", datetime, barcode);
                    Helper.AddClipboard(barcode);
                    return string.Format("{0}\r\nClipboard\r\n{1}", barcode, datetime);
                };
            After += nancyContext =>
                {
                };
        }
    }
}

