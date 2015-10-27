using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.IO;
using System.Reflection;

namespace ScanAndREST
{
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();

            BindingContext = this;

            webView.Navigating += (object sender, WebNavigatingEventArgs e) =>
            {
                if (Uri.IsWellFormedUriString(e.Url, UriKind.Absolute) && !e.Url.StartsWith("file"))
                {
                    Device.OpenUri(new Uri(e.Url));
                    e.Cancel = true;
                }
            };
        }

        public HtmlWebViewSource InfoHtml
        {
            get
            {
                var assembly = typeof(InfoPage).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("ScanAndREST.Resources.Html.Info.html");
                StreamReader reader = new StreamReader(stream);
                string htmlString = reader.ReadToEnd();

                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = htmlString;
                return htmlSource;
            }
        }
    }
}
