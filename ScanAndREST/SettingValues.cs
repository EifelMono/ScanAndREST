using System;
using Newtonsoft.Json;

namespace ScanAndREST
{
    public class SettingValues
    {
        public SettingValues()
        {
        }

        public string Name { get; set;}

        public bool Default { get; set; }

        public string ScannerTopText { get; set; }= "Hold your camera over the barcode";

        public string ScannerBottomText { get; set; }= "Scanning will happen automatically";

        public string ScannerCancelText { get; set; }= "Cancel";

        public string ScannerFlashText { get; set; }= "Flash";

        public string RESTUrl { get; set; }

        [JsonIgnore]
        public string RESTUrlBase
        {
            get
            { 
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
                return RESTUrl.Substring(RESTUrlBase.Length+ 1);
            }
        }
    }
}

