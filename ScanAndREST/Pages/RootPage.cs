using System;
using System.Linq;

using Xamarin.Forms;

namespace ScanAndREST
{
    public class RootPage : MasterDetailPage
    {
        public RootPage()
        {
            var menuPage = new MenuPage();
   
            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuItem);

            Master = menuPage;

            Globals.Settings.ChangAndRebuild();

            var page = new ScanPage();
            page.CurrentSettingValues = Globals.Settings.Items.FirstOrDefault((s) => s.Default);

            Detail = new NavigationPage(page);
        }

        void NavigateTo(MenuItem menu)
        {
            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

            displayPage.Title = menu.Title;

            if (displayPage is ScanPage)
                (displayPage as ScanPage).CurrentSettingValues = Globals.Settings.Items.FirstOrDefault((i) => i.Name == menu.Title);

            Detail = new NavigationPage(displayPage);

            IsPresented = false;
        }
    }
}


