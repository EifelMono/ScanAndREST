using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Text;
using ZXing.Mobile;
using System.Threading.Tasks;
using RestSharp.Portable;
using Media.Plugin;
using System.Diagnostics;

namespace ScanAndREST
{
    public partial class ScanPage : ContentPage
    {
        SettingValues CurrentSettings = new SettingValues();
        ToolbarItem toolbarItemSettings = null;

        public ScanPage()
        {
            InitializeComponent();

            #region Design
            ToolbarItems.Add(toolbarItemSettings = new ToolbarItem("change", "Icons/Settings.png", new Action(() =>
                        {
                            Navigation.PushAsync(new SettingsPage(), true);
                        })));
            
            labelBarcode.TextColor = Globals.Color.Barcode;
            labelBarcode.FontAttributes = FontAttributes.Bold;
            labelBarcode.HorizontalOptions = LayoutOptions.CenterAndExpand;
            labelBarcode.VerticalOptions = LayoutOptions.CenterAndExpand;

            labelResult.TextColor = Globals.Color.REST;
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
            TapGestureRecognizer gesture = new TapGestureRecognizer();
            gesture.Tapped += circleImageStartTapped;
            circleImageStart.GestureRecognizers.Add(gesture);
            #endregion
        }

        static Encoding BodyEncoding = new System.Text.UTF8Encoding();
        static MobileBarcodeScanner scanner = null;

        async void circleImageStartTapped(object sender, EventArgs args)
        {
            if (circleImageStart.BorderColor == Color.Red)
                return;
            labelBarcode.Text = "";
            labelResult.Text = "";
            circleImageStart.BorderColor = Color.Red;
            try
            {
                if (CrossMedia.Current.IsCameraAvailable)
                {
                    // The root page of your application
                    if (scanner == null)
                        scanner = new MobileBarcodeScanner();
                    scanner.TopText = CurrentSettings.ScannerTopText;
                    scanner.BottomText = CurrentSettings.ScannerBottomText;
                    scanner.CancelButtonText = CurrentSettings.ScannerCancelText;
                    scanner.FlashButtonText = CurrentSettings.ScannerFlashText;

                    MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions();

                    options.PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.CODE_39, ZXing.BarcodeFormat.DATA_MATRIX, ZXing.BarcodeFormat.EAN_13, ZXing.BarcodeFormat.EAN_8 };
                    options.AutoRotate = false;
                    options.TryHarder = true;
                    options.TryInverted = true;
                    options.BuildMultiFormatReader();
                    // options.PureBarcode = true;

                    var result = await scanner.Scan(options);

                    if (result == null)
                        return; 
                    labelBarcode.Text = result.Text;
                    Task.Run(async() =>
                        {
                            Debug.WriteLine("x");
                            // Vibrate.Current.Vibration();
                        });
                }
                else
                    labelBarcode.Text = "No camera avaiable";

                if (!string.IsNullOrEmpty(CurrentSettings.RESTUrlBase))
                    try
                    {
                        var client = new RestClient(CurrentSettings.RESTUrlBase);
                        client.Timeout = new TimeSpan(0, 0, 2);

                        RestRequest request = new RestRequest(CurrentSettings.RESTUrlResource, System.Net.Http.HttpMethod.Post);
                        request.AddJsonBody(labelBarcode.Text);
                        var response = (await client.Execute(request));
                        var resultCount = BodyEncoding.GetString(response.RawBytes, 0, response.RawBytes.Length);
                        labelResult.Text = resultCount;
                    }
                    catch
                    {
                        labelResult.Text = "No anser from the REST Service"; 
                    }
            }
            finally
            {
                circleImageStart.BorderColor = Color.Black;
            }
        }
    }
}

