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
            MenuPage= new MenuPage();
   
            MenuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuItem);

            Master = MenuPage;

            Globals.Settings.ChangAndRebuild();

            var page = new ScanPage();
            page.CurrentSettingValues = Globals.Settings.Items.FirstOrDefault((s) => s.Default);

            Detail = new NavigationPage(page);
        }

        public void NavigateTo(MenuItem menu)
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
    }
}


