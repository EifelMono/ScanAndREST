using System;
using System.Linq;

using Xamarin.Forms;

namespace ScanAndREST
{
    public class RootPage : MasterDetailPage
    {
        MenuPage MenuPage;

        public RootPage()
        {
            MenuPage = new MenuPage();
   
            MenuPage.Menu.ItemSelected += (sender, e) => NavigateToMenu(e.SelectedItem as MenuItem);

            Master = MenuPage;

            Globals.Settings.ChangAndRebuild();

            var page = new ScanPage();
            var useSettings = Globals.Settings.Items.FirstOrDefault((s) => s.Default);
            if (useSettings == null && Globals.Settings.Items.Count > 0)
            {
                useSettings = Globals.Settings.Items[0];
                useSettings.Default = true;
                Globals.Settings.Write();
            }
            page.CurrentSettingValues = useSettings;
            Detail = new NavigationPage(page);
        }

        public void NavigateToMenu(MenuItem menu)
        {
            if (menu == null)
                menu = MenuPage.MenuItems.Last();
            
            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

            displayPage.Title = menu.Title;

            if (displayPage is ScanPage)
                (displayPage as ScanPage).CurrentSettingValues = Globals.Settings.Items.FirstOrDefault((i) => i.Name == menu.Title);

            Detail = new NavigationPage(displayPage);

            IsPresented = false;
        }

        public void NavigateToPage(Page page)
        {
            Detail = new NavigationPage(page);

            IsPresented = false;
        }
    }
}


