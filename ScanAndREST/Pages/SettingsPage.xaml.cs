using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ScanAndREST
{
    public partial class SettingsPage : CarouselPage
    {
        public SettingsPage()
        {
            Title = "Settings";
            InitializeComponent();
            for (int i = 0; i < 10; i++)
                Children.Add(new SettingPage
                    {
                        Title = "Setting "+ i.ToString()
                    });
        }

        protected override void OnCurrentPageChanged()
        {
            Title = CurrentPage.Title;
        }
    }
}

