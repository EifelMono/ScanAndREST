using System;

using Xamarin.Forms;
using System.Collections.Generic;
using PCLStorage;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ScanAndREST
{
    public class SettingsValues
    {
        public List<SettingValues> Items { get; set; }= new List<SettingValues>();

        IFolder Folder = FileSystem.Current.LocalStorage;
        IFile File = null;
        string FileName = "Settings.json";

        public async Task  Read()
        {
            try
            {
                if (File == null)
                    File = await Folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
                Items = JsonConvert.DeserializeObject<List<SettingValues>>(await File.ReadAllTextAsync());
            }
            catch
            {
                Items = null;
            }
            if (Items == null || Items.Count == 0)
                LoadDefaults();

            ChangAndRebuild();
        }

        public  async void Write()
        {
            try
            {
                if (File == null)
                    File = await Folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
                await File.WriteAllTextAsync(JsonConvert.SerializeObject(Items, Formatting.Indented));
            }
            catch
            {
            }
            ChangAndRebuild();
        }

        public void LoadDefaults()
        {
            Items = new List<SettingValues>
            {
                new SettingValues
                {
                    Name = "Scan Only",
                    Default = false,
                    RESTUrl = "",

                },
                new SettingValues
                {
                    Name = "Scan and REST TestServer",
                    Default = false,
                    RESTUrl = "http://localhost:9876/Barcode"
                },
                new SettingValues
                {
                    Name = "OCR",
                    Default = true,
                    RESTUrl = "http://lwdeu089kbdk32:14261/Master/Find/Barcode"
                }
            };
        }

        public Action NotifyEvent = null;

        public void ChangAndRebuild()
        {
            if (NotifyEvent != null)
                NotifyEvent();
        }
    }
}


