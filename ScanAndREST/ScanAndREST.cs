using System;

using Xamarin.Forms;

namespace ScanAndREST
{
    public class App : Application
    {
        public App()
        {
            Globals.Settings.Read();
            MainPage = new RootPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

