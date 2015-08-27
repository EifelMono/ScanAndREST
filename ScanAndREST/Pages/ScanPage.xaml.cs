using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Text;
using ZXing.Mobile;
using Refractored.Xam.Vibrate;
using System.Threading.Tasks;
using RestSharp.Portable;
using System.Diagnostics;
using Media.Plugin;

namespace ScanAndREST
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();

            Encoding BodyEncoding = new System.Text.UTF8Encoding();
            MobileBarcodeScanner scanner = null;
            MobileBarcodeScanningOptions options = null;

            var label = new Label() { HorizontalOptions = LayoutOptions.Center };

            var btnScan = new Button { Text = "Scan BarCode" };
            btnScan.Clicked += async (sender, args) =>
            {
                string barcode = null;
                if (CrossMedia.Current.IsCameraAvailable)
                {
                    // The root page of your application
                    if (scanner == null)
                        scanner = new MobileBarcodeScanner();
                    scanner.TopText = "Hold your camera about \n6 inches away from the barcode";
                    scanner.BottomText = "Scanning will happen automatically";
                    scanner.CancelButtonText = "Cancel";

                    if (options == null)
                        options = new MobileBarcodeScanningOptions();
                    options.PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.CODE_39, ZXing.BarcodeFormat.DATA_MATRIX, ZXing.BarcodeFormat.EAN_13, ZXing.BarcodeFormat.EAN_8 };
                    options.AutoRotate = false;
                    options.TryHarder = false;
                    options.TryInverted = true;
                    options.BuildMultiFormatReader();
                    // options.PureBarcode = true;


                    var result = await scanner.Scan(options);

                    if (result == null)
                        return; 
                    barcode = result.Text;
                    Task.Run(async() =>
                        {
                            CrossVibrate.Current.Vibration();
                        });
                }
                else
                {
                    barcode = "No Camera Barcode";
                }

                label.Text = barcode;
                try
                {
                    var client = new RestClient("http://localhost:9876");
                    client.Timeout = new TimeSpan(0, 0, 2);

                    // RestRequest request = new RestRequest("Barcode", System.Net.Http.HttpMethod.Post);
                    RestRequest request = new RestRequest("Other/Barcode", System.Net.Http.HttpMethod.Post);
                    request.AddJsonBody(barcode);
                    var response = (await client.Execute(request));
                    var resultCount = BodyEncoding.GetString(response.RawBytes, 0, response.RawBytes.Length);
                    label.Text += " = " + resultCount;
                }
                catch (Exception ex)
                {
                    label.Text += " = " + "now anser from the REST Servie"; 
                    Debug.WriteLine(ex.ToString());
                }
            };
            Content = new StackLayout
            {
                Padding = new Thickness(10, 300, 10, 10),
                Children =
                {
                    new Label(),
                    btnScan,
                    new Label(),
                    label
                }
            };
        }
    }
}

