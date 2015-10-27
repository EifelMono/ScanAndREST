using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Text;
using ZXing.Mobile;
using System.Threading.Tasks;
using RestSharp.Portable;
using System.Diagnostics;
using RestSharp.Portable.HttpClient;

namespace ScanAndREST
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem("change", "Icons/Settings.png", new Action(() =>
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
            string Barcode = "eifelmono";

            circleImageStart.BorderColor = Color.Red;
            try
            {
                if (Backdoor.IsCameraAvailable)
                {
                    if (scanner == null)
                        scanner = new MobileBarcodeScanner();
                    scanner.TopText = CurrentSettingValues.ScannerTopText;
                    scanner.BottomText = CurrentSettingValues.ScannerBottomText;
                    scanner.CancelButtonText = CurrentSettingValues.ScannerCancelText;
                    scanner.FlashButtonText = CurrentSettingValues.ScannerFlashText;

                    MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions
                    {
                        PossibleFormats = CurrentSettingValues.BarcodeFormatsAsList,
                        AutoRotate = CurrentSettingValues.AutoRotate,
                        TryHarder = CurrentSettingValues.TryHarder,
                        TryInverted = CurrentSettingValues.TryInverted,
                        PureBarcode = CurrentSettingValues.PureBarcode
                    };
                    // options.BuildMultiFormatReader();
                 
                    var result = await scanner.Scan(options);
                    if (result == null)
                        return; 
                    Barcode = result.Text;
                }
                labelBarcode.Text = Barcode;

                Task.Run(() =>
                    {
                        Backdoor.Vibrate();
                    });

                if (!string.IsNullOrEmpty(CurrentSettingValues.RESTUrlBase))
                    try
                    {
                        circleImageStart.Source = ImageSource.FromResource("ScanAndREST.Resources.Icons.ScanAndRESTResult.png");
                        var client = new RestClient(CurrentSettingValues.RESTUrlBase);
                        client.Timeout = new TimeSpan(0, 0, CurrentSettingValues.RESTTimeout);
                        RestRequest request = new RestRequest(CurrentSettingValues.RESTUrlResource, RestSharp.Portable.Method.GET);
                        request.AddParameter("Barcode", Barcode);
                        var response = (await client.Execute(request));
                        labelResult.Text = BodyEncoding.GetString(response.RawBytes, 0, response.RawBytes.Length);
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


        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = CurrentSettingValues.Name;
            if (CurrentSettingValues.Delete)
            {
                Globals.Settings.Items.Remove(CurrentSettingValues);
                Globals.Settings.ChangAndRebuild();
                (App.Current.MainPage as RootPage).NavigateToMenu(null);
            }
            Globals.Settings.Default(CurrentSettingValues);
            Globals.Settings.Write();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}

