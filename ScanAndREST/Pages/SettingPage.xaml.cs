using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ScanAndREST
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();

            Button button = new Button()
            {
                Text = "Hallo",
                Command = new Command((x) =>
                    {
                        Navigation.PopAsync(true);
                    })
            };

            Content = button;
        }
    }
}

