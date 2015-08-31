using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Text;
using ZXing.Mobile;
using System.Threading.Tasks;
using RestSharp.Portable;
using System.Diagnostics;

namespace ScanAndREST
{
    public partial class ScanPage : ContentPage
    {
        ToolbarItem toolbarItemSettings = null;

        public ScanPage()
        {
            InitializeComponent();

            ToolbarItems.Add(toolbarItemSettings = new ToolbarItem("change", "Icons/Settings.png", new Action(() =>
                        {
                            Navigation.PushAsync(new SettingPage { SettingValues = CurrentSettingValues }, true);
                        })));

            labelBarcode.TextColor = Globals.Color.Barcode;
            labelBarcode.FontAttributes = FontAttributes.Bold;
            labelBarcode.FontSize = 24;
            labelBarcode.HorizontalOptions = LayoutOptions.CenterAndExpand;
            labelBarcode.VerticalOptions = LayoutOptions.CenterAndExpand;

            labelResult.TextColor = Globals.Color.REST;
            labelResult.FontAttributes = FontAttributes.Bold;
            labelResult.FontSize = 24;
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
        }

        #region Scan Clicked

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
                if (Backdoor.IsCameraAvailable)
                {
                    // The root page of your application
                    if (scanner == null)
                        scanner = new MobileBarcodeScanner();
                    scanner.TopText = CurrentSettingValues.ScannerTopText;
                    scanner.BottomText = CurrentSettingValues.ScannerBottomText;
                    scanner.CancelButtonText = CurrentSettingValues.ScannerCancelText;
                    scanner.FlashButtonText = CurrentSettingValues.ScannerFlashText;

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
                    Task.Run(() =>
                        {
                            Backdoor.Vibrate();
                        });
                }
                else
                    labelBarcode.Text = "No camera avaiable";

                if (!string.IsNullOrEmpty(CurrentSettingValues.RESTUrlBase))
                    try
                    {
                        circleImageStart.Source = ImageSource.FromResource("ScanAndREST.Resources.Icons.ScanAndRESTResult.png");
                        var client = new RestClient(CurrentSettingValues.RESTUrlBase);
                        client.Timeout = new TimeSpan(0, 0, 2);

                        RestRequest request = new RestRequest(CurrentSettingValues.RESTUrlResource, System.Net.Http.HttpMethod.Post);
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
                circleImageStart.Source = ImageSource.FromResource("ScanAndREST.Resources.Icons.ScanAndRESTStart.png");
            }
        }

        #endregion

        #region Properties

        protected SettingValues m_CurrentSettingValues = null;

        public SettingValues CurrentSettingValues
        {
            get
            {
                if (m_CurrentSettingValues == null)
                    m_CurrentSettingValues = new SettingValues();
                return m_CurrentSettingValues;
            }
            set
            {
                m_CurrentSettingValues = value;
                Title = CurrentSettingValues.Name;
            }
        }

        #endregion

    }
}

