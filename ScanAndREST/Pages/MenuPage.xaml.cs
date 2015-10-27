using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace ScanAndREST
{
    public partial class MenuPage : ContentPage
    {
        public List<MenuItem> MenuItems { get; set; }

        public delegate void MenuSelectedDelegate(ScanAndREST.MenuItem item);

        public MenuSelectedDelegate OnMenuSelected { get; set; }

        public MenuPage()
        {
            InitializeComponent();

            Icon = "Icons/menu.png";
            Title = "menu"; 

            Globals.Settings.NotifyEvent = Changes;

            var tgr = new TapGestureRecognizer();
            tgr.Tapped += (sender, e) => (App.Current.MainPage as RootPage).NavigateToPage(new InfoPage());
            gridInfo.GestureRecognizers.Add(tgr); 

            listViewMenu.ItemSelected += (s, e) =>
            {
                if (OnMenuSelected != null)
                {
                    var item = listViewMenu.SelectedItem as MenuItem;
                    if (item != null)
                    {
                        if (item.Add)
                        {
                            Globals.Settings.Items.Add(new SettingValues());
                            Globals.Settings.ChangAndRebuild();
                            (App.Current.MainPage as RootPage).NavigateToMenu(null);
                            Globals.Settings.Write(); 
                        }
                        else
                            OnMenuSelected(item);
                    }
                }
            };
            BindingContext = this;
        }

        protected void Changes()
        {
            if (Globals.Settings.Items == null)
                return;
            MenuItems = new List<MenuItem>();
            foreach (var item in Globals.Settings.Items)
            {
                MenuItems.Add(new MenuItem
                    {
                        IconSource = item.Default ? "Icons/ScanAndREST Transparent.png" : "",
                        Title = item.Name,
                        TargetType = typeof(ScanPage),
                    });
            }
            MenuItems.Add(new MenuItem
                {
                    IconSource = "Icons/Plus.png",
                    Title = "Add",
                    Add = true
                });
            listViewMenu.ItemsSource = MenuItems;
  
            OnPropertyChanged(nameof(MenuItems));
        }

        public MenuItem Last()
        {
            return MenuItems[MenuItems.Count - 2];
        }
    }

    public class MenuItem
    {
        public string IconSource { get; set; }= null;

        public string Title { get; set; }= "";

        public Type TargetType { get; set; }= null;

        public bool Add { get; set; }= false;

        public override string ToString()
        {
            return string.Format("[MenuItem: IconSource={0}, Title={1}, TargetType={2}, Add={3}]", IconSource, Title, TargetType, Add);
        }
    }
}
