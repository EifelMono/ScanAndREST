using System;

using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace ScanAndREST
{
    public class App : Application
    {
        public App()
        {
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

