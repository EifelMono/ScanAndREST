using System;

using Xamarin.Forms;
using System.Collections.Generic;
using PCLStorage;
using Newtonsoft.Json;

namespace ScanAndREST
{
    public class SettingsValues
    {
        public List<SettingValues> Items { get; set; }= new List<SettingValues>();

        IFolder Folder = FileSystem.Current.LocalStorage;
        IFile File = null;
        string FileName = "Settings.json";

        public async void Read()
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

            Changed();
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
            Changed();
        }

        public void LoadDefaults()
        {
            Items = new List<SettingValues>
            {
                new SettingValues
                {
                    Name = "Scan Only",
                    Default = true,
                    RESTUrl = "",

                },
                new SettingValues
                {
                    Name = "Scan and REST TestServer",
                    RESTUrl = "http://localhost:9876/Barcode"
                }
            };
        }

        public Action NotifyEvent = null;

        public void Changed()
        {
            if (NotifyEvent != null)
                NotifyEvent();
        }
    }
}


