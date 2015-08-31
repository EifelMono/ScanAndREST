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
            ToolbarItems.Add(new ToolbarItem("Ok", "Icons/Ok.png", new Action(() =>
                        {
                            Navigation.PopAsync(true);
                            Globals.Settings.Write();
                        })));
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
            }
        }

        public int Nr { get; set; }
    }
}

