using System;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace ScanAndREST
{
    public class RootPage : MasterDetailPage
    {
        MenuPage MenuPage;

        public RootPage()
        {
            MenuPage = new MenuPage();
   
            MenuPage.OnMenuSelected += (menuItem) => NavigateToMenu(menuItem);
            MenuPage.IsEnabled = false;

            Master = MenuPage;
        
            var page = new ScanPage();
            LoadSettings(page);
           
            Detail = new NavigationPage(new ContentPage {
                Content= new Grid {
                    Children= {
                        new ActivityIndicator{
                            IsRunning= true
                        }
                    }
                }
            });
            NavigationPage.SetHasNavigationBar(Detail, false);
        }

        async void LoadSettings(ScanPage page)
        {
            await Globals.Settings.Read();
            Globals.Settings.ChangAndRebuild();

            var useSettings = Globals.Settings.Items.FirstOrDefault((s) => s.Default);
            if (useSettings == null && Globals.Settings.Items.Count > 0)
            {
                useSettings = Globals.Settings.Items[0];
                useSettings.Default = true;
                Globals.Settings.Write();
            }
            page.CurrentSettingValues = useSettings; 
            Detail = new NavigationPage(page);
            NavigationPage.SetHasNavigationBar(Detail, true);
        }

		public Page NavigateToMenu(MenuItem menu)
        {
            if (menu == null)
                menu = MenuPage.Last();
            
            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

            displayPage.Title = menu.Title;

            if (displayPage is ScanPage)
                (displayPage as ScanPage).CurrentSettingValues = Globals.Settings.Items.FirstOrDefault((i) => i.Name == menu.Title);

            Detail = new NavigationPage(displayPage);

            IsPresented = false;

			return displayPage;
        }

        public void NavigateToPage(Page page)
        {
            Detail = new NavigationPage(page);

            IsPresented = false;
        }
    }
}


