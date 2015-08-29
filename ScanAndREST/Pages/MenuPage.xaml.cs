using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ScanAndREST
{
    public partial class MenuPage : ContentPage
    {
        public ListView Menu { get; set; }

        public MenuPage()
        {
            InitializeComponent();

            Icon= "Icons/menu.png";
            Title = "menu"; 
            BackgroundColor = Color.Gray;

            DataTemplate cell;
            Menu = new ListView()
                {
                    
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.Transparent,
                    ItemTemplate= (cell = new DataTemplate(typeof(ImageCell))),
                };
            cell.SetBinding(TextCell.TextProperty, "Title");
            cell.SetBinding(TextCell.TextColorProperty, "White");
            cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");

            Globals.Settings.NotifyEvent = Changes;

            var menuLabel = new ContentView
            {
                Padding = new Thickness(10, 36, 0, 5),
                Content = new Label
                {
                    TextColor = Color.FromHex("AAAAAA"),
                    Text = "Select your REST interface", 
                }
            };

            var layout = new StackLayout
            { 
                Spacing = 0, 
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(menuLabel);
            layout.Children.Add(Menu);

            Content = layout;
        }

        protected void Changes()
        {
            var list = new List<MenuItem>();
            foreach(var item in Globals.Settings.Items)
            {
                list.Add(new MenuItem
                    {
                        Title = item.Name,
                        IconSource = "",
                        TargetType = typeof(ScanPage),
                    });
            }
            Menu.ItemsSource = list;
        }
    }

    public class MenuItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }
}

