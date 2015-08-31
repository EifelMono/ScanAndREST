using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Linq;

namespace ScanAndREST
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        SettingValues m_SettingValues;

        public SettingValues SettingValues
        {
            get
            {
                return m_SettingValues;
            }
            set
            {
                m_SettingValues = value;
                BindingContext = m_SettingValues;

                tableSectionBarcodeFormats.Clear();
                for (int i = 0; i < m_SettingValues.BarcodeFormats.Count; i++)
                {
                    var kv = m_SettingValues.BarcodeFormats.ElementAt(i);
                    var cell = new SwitchCell();
                    cell.Text = kv.Key;
                    string path = string.Format("BarcodeFormats[{0}]", kv.Key);
                    cell.SetBinding(SwitchCell.OnProperty, path);
                    tableSectionBarcodeFormats.Add(cell);
                }

                if (m_SettingValues.Deleteable)
                {
                    var tableSection = new TableSection { Title = "Action" };
                    tableRoot.Add(tableSection);
                    var cell = new ViewCell();
                    tableSection.Add(cell);

                    var button = new Button
                    {
                        Text = "Delete",
                        BackgroundColor = Color.Red
                    };
                    button.Image = "Icons/Minus.png";

                    button.Clicked += async (object sender, EventArgs e) =>
                    {
                            if (await DisplayAlert("Query", string.Format("Delete {0}", SettingValues.Name), "Ok", "Cancel"))
                        {
                            m_SettingValues.Delete = true;
                            Navigation.PopAsync(true); 
                        }
                    };

                    cell.View = new ContentView
                    {
                        Padding = new Thickness(10, 5, 10, 5),
                        Content = button
                    };
                }
            }
        }

        public int Nr { get; set; }
    }
}

