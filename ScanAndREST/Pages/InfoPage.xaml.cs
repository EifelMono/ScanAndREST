using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ScanAndREST
{
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();
            labelGitHub.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command((o) =>
                        {
                            Backdoor.WebOpen("https://github.com/EifelMono/ScanAndREST");
                        })
                });
        }
    }
}

