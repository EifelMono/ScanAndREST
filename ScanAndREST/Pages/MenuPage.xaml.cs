using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Linq;

namespace ScanAndREST
{
    public partial class MenuPage : ContentPage
    {
        public ListView Menu { get; set; }

        public List<MenuItem> MenuItems { get; set; }

        public MenuPage()
        {
            InitializeComponent();

            Icon = "Icons/menu.png";
            Title = "menu"; 
            BackgroundColor = Color.Gray;

            DataTemplate cell;
            Menu = new ListView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
                ItemTemplate = (cell = new DataTemplate(typeof(ImageCell))),
            };
            cell.SetBinding(TextCell.TextProperty, "Title");
            cell.SetBinding(TextCell.TextColorProperty, "White");
            cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");

            Globals.Settings.NotifyEvent = Changes;

            var buttonAdd = new Button();
            buttonAdd.VerticalOptions = LayoutOptions.Center;
            buttonAdd.Image = "Icons/Plus@3x.png";
            buttonAdd.Clicked += (object sender, EventArgs e) =>
            {
                Globals.Settings.Items.Add(new SettingValues());
                Globals.Settings.ChangAndRebuild();
                (App.Current.MainPage as RootPage).NavigateToMenu(null);
                Globals.Settings.Write();
            };

            var menuHeader = new ContentView
            {
                Padding = new Thickness(10, 36, 0, 5),
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        buttonAdd,
                        new Label
                        {
                            TextColor = Color.FromHex("AAAAAA"),
                            Text = "Select your configuration ...", 
                            VerticalOptions = LayoutOptions.Center,
                        },
                    }
                }
            };

            var buttonInfo = new Button();
            buttonInfo.VerticalOptions = LayoutOptions.Center;
            buttonInfo.Image = "Icons/Info@3x.png";
            buttonInfo.Clicked += (object sender, EventArgs e) =>
            {
                    (App.Current.MainPage as RootPage).NavigateToPage(new InfoPage());
            };
            

            var menuFooter = new ContentView
            {
                Padding = new Thickness(10, 5, 0, 5),
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        buttonInfo,
                        new Label
                        {
                            TextColor = Color.FromHex("AAAAAA"),
                            Text = "ScanAndREST ...", 
                            VerticalOptions = LayoutOptions.Center,
                        },
                    }
                }
            };

            var layout = new StackLayout
            { 
                Spacing = 0, 
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(menuHeader);
            layout.Children.Add(Menu);
            layout.Children.Add(menuFooter);
            Content = layout;
        }

        protected void Changes()
        {
            MenuItems = new List<MenuItem>();
            foreach (var item in Globals.Settings.Items)
            {
                MenuItems.Add(new MenuItem
                    {
                        Title = item.Name,
                        IconSource = "",
                        TargetType = typeof(ScanPage),
                    });
            }
            Menu.ItemsSource = MenuItems;
        }
    }

    public class MenuItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }
}

