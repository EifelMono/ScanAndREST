using System;
using Xamarin.Forms;

namespace ScanAndREST
{
    public static class Globals
    {
        public static class Color
        {
            public static Xamarin.Forms.Color Barcode { get; set; }= Xamarin.Forms.Color.FromHex("66CCFF");

            public static Xamarin.Forms.Color REST{ get; set; }= Xamarin.Forms.Color.FromHex("66FFCC");
        }

        static SettingsValues m_Settings;

        public static SettingsValues Settings
        {
            get
            {
                if (m_Settings == null)
                    m_Settings = new SettingsValues();
                return m_Settings;
            }
        }
    }
}

