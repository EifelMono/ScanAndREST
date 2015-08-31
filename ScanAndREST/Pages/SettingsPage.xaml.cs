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
            for (int nr = 0; nr < Globals.Settings.Items.Count; nr++)
            {
                Children.Add(new SettingPage
                    {
                        Nr = nr,
                        Title = Globals.Settings.Items[nr].Name,
                        SettingValues = Globals.Settings.Items[nr]
                    });
            }
        }

        protected override void OnCurrentPageChanged()
        {
            Title = CurrentPage.Title;
        }
    }
}

