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
using ImageCircle.Forms.Plugin.Abstractions;

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

            labelBarcode.TextColor = Color.FromHex("66CCFF");
            labelBarcode.FontAttributes = FontAttributes.Bold;
            labelBarcode.HorizontalOptions = LayoutOptions.CenterAndExpand;
            labelBarcode.VerticalOptions = LayoutOptions.CenterAndExpand;


            labelResult.TextColor = Color.FromHex("66FFCC");
            labelResult.FontAttributes = FontAttributes.Bold;
            labelResult.HorizontalOptions = LayoutOptions.CenterAndExpand;
            labelResult.VerticalOptions = LayoutOptions.CenterAndExpand;

            circleImageStart.BackgroundColor = Color.Red;
            circleImageStart.BorderColor = Color.Black;
            circleImageStart.BorderThickness = 5;
            circleImageStart.Source = ImageSource.FromResource("ScanAndREST.Resources.Icons.ScanAndRESTStart.png");
            circleImageStart.Aspect = Aspect.AspectFill;
            circleImageStart.BindingContext = this;
            circleImageStart.HeightRequest = circleImageStart.Width;
            circleImageStart.HorizontalOptions = LayoutOptions.CenterAndExpand;
            circleImageStart.VerticalOptions = LayoutOptions.CenterAndExpand;
            circleImageStart.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    TappedCallback = async (v, o) =>
                    {
                        if (circleImageStart.BorderColor == Color.Red)
                            return;
                        labelBarcode.Text = "";
                        labelResult.Text = "";
                        circleImageStart.BorderColor = Color.Red;
                        try
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

                            labelBarcode.Text = barcode;
                            try
                            {
                                var client = new RestClient("http://localhost:9876");
                                client.Timeout = new TimeSpan(0, 0, 2);
                                
                                // RestRequest request = new RestRequest("Barcode", System.Net.Http.HttpMethod.Post);
                                RestRequest request = new RestRequest("Other/Barcode", System.Net.Http.HttpMethod.Post);
                                request.AddJsonBody(barcode);
                                var response = (await client.Execute(request));
                                var resultCount = BodyEncoding.GetString(response.RawBytes, 0, response.RawBytes.Length);
                                labelResult.Text = resultCount;
                            }
                            catch (Exception ex)
                            {
                                labelResult.Text = "now anser from the REST Servie"; 
                            }
                        }
                        finally
                        {
                            circleImageStart.BorderColor = Color.Black;
                        }
                    },
                });
        }
    }
}

