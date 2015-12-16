using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ScanAndREST
{
    public static class Extensions
    {
        public static T ToEnum<T>(this string value) where T: struct
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }

    public class SettingValues
    {
        #region Control

        public string Name { get; set; } = "Scanner"+ DateTime.Now.ToString();

        public bool Default { get; set; }= false;

        public bool BarcodeFormatVisible { get; set; }= false;

        public bool Deleteable { get; set; }= true;

        [JsonIgnore]

        public bool Delete { get; set; }= false;

        #endregion

        #region Text

        public string ScannerTopText { get; set; }= "Hold your camera over the barcode";

        public string ScannerBottomText { get; set; }= "Scanning will happen automatically";

        public string ScannerCancelText { get; set; }= "Cancel";

        public string ScannerFlashText { get; set; }= "Flash";

        #endregion

        #region REST

        [JsonProperty("RESTTimeout")]
        protected int m_RESTTimeout { get; set; }= 1;

        [JsonIgnore]
        public int RESTTimeout
        {
            get
            {
                if (m_RESTTimeout <= 0)
                    m_RESTTimeout = 1;
                return m_RESTTimeout;
            }
            set
            {
                m_RESTTimeout = value;
            }
        }

        public string RESTUrl { get; set; }

        [JsonIgnore]
        public string RESTUrlBase
        {
            get
            { 
                if (string.IsNullOrEmpty(RESTUrl))
                    return "";
                var parts = RESTUrl.Trim().Split('/');
                if (parts.Length > 3 & string.IsNullOrEmpty(parts[1]))
                {
                    return parts[0] + "//" + parts[2];
                }
                return RESTUrl;
            }
        }

        [JsonIgnore]
        public string RESTUrlResource
        {
            get
            {
                if (string.IsNullOrEmpty(RESTUrl))
                    return "";
                return RESTUrl.Substring(RESTUrlBase.Length + 1);
            }
        }

        #endregion

        #region Barcode

        [JsonIgnore]
        public List<ZXing.BarcodeFormat> BarcodeFormatsAsList
        {
            get
            {
                var list = new List<ZXing.BarcodeFormat>();
                foreach (var item in BarcodeFormats)
                    if (item.Value)
                        list.Add(item.Key.ToEnum<ZXing.BarcodeFormat>());
                return list;
            }
        }

        private Dictionary<string, bool> m_BarcodeFormats = null;

        public Dictionary<string, bool> BarcodeFormats
        {
            get
            {
                if (m_BarcodeFormats == null)
                {
                    m_BarcodeFormats = new Dictionary<string, bool>();
                    foreach (ZXing.BarcodeFormat item in Enum.GetValues(typeof(ZXing.BarcodeFormat)))
                        if (item != ZXing.BarcodeFormat.All_1D)
                            m_BarcodeFormats.Add(item.ToString(), false);

                    m_BarcodeFormats[ZXing.BarcodeFormat.CODE_39.ToString()] = true;
                    m_BarcodeFormats[ZXing.BarcodeFormat.EAN_13.ToString()] = true;
                    m_BarcodeFormats[ZXing.BarcodeFormat.EAN_8.ToString()] = true;
                    m_BarcodeFormats[ZXing.BarcodeFormat.CODE_128.ToString()] = true;
                    m_BarcodeFormats[ZXing.BarcodeFormat.QR_CODE.ToString()] = true;
                    m_BarcodeFormats[ZXing.BarcodeFormat.DATA_MATRIX.ToString()] = true;
                }
                return m_BarcodeFormats;
            }
            set
            {
                m_BarcodeFormats = value;
            }
        }

        public bool AutoRotate { get; set; }= false;

        public bool TryHarder { get; set; }= true;

        public bool TryInverted { get; set; }= true;

        public bool PureBarcode { get; set; }= false;

        #endregion
    }
}

