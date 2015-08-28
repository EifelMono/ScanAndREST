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

            Icon = "settings.png";
            Title = "menu"; // The Title property must be set.
            BackgroundColor = Color.FromHex("333333");

            Menu = new MenuListView();

            var menuLabel = new ContentView
            {
                Padding = new Thickness(10, 36, 0, 5),
                Content = new Label
                {
                    TextColor = Color.FromHex("AAAAAA"),
                    Text = "MENU", 
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
    }

    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            this.Add(new MenuItem()
                { 
                    Title = "Contracts",
                    IconSource = "contracts.png", 
                    TargetType = typeof(ScanPage)
                });

            this.Add(new MenuItem()
                { 
                    Title = "Leads", 
                    IconSource = "Lead.png", 
                    TargetType = typeof(ScanPage)
                });

            this.Add(new MenuItem()
                { 
                    Title = "Accounts", 
                    IconSource = "Accounts.png", 
                    TargetType = typeof(ScanPage)
                });

            this.Add(new MenuItem()
                {
                    Title = "Opportunities",
                    IconSource = "Opportunity.png",
                    TargetType = typeof(ScanPage)
                });
        }
    }

    public class MenuItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }

    public class MenuListView : ListView
    {
        public MenuListView()
        {
            List<MenuItem> data = new MenuListData();

            ItemsSource = data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;

            var cell = new DataTemplate(typeof(ImageCell));
            cell.SetBinding(TextCell.TextProperty, "Title");
            cell.SetBinding(TextCell.TextColorProperty, "White");
            cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");

            ItemTemplate = cell;
        }
    }

}

