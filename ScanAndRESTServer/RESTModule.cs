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
                    var barcode = (string)Request.Query.Barcode;
                    return string.Format("{0}\r\nHello World\r\n{1}", barcode, DateTime.Now.ToString());
                };

            Get["/ScanToClipboard"] = parameters =>
                {
                    var barcode = (string)Request.Query.Barcode;
                    Helper.ClipboarCopy(barcode);
                    return string.Format("{0}\r\nClipboard\r\n{1}", barcode, DateTime.Now.ToString());
                };
            After += nancyContext =>
                {
                };
        }
    }
}

