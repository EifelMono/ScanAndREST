using System;

using Xamarin.Forms;
using System.Collections.Generic;
using PCLStorage;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ScanAndREST
{
    public class SettingsValues
    {
        public List<SettingValues> Items { get; set; }= new List<SettingValues>();

        IFolder Folder = FileSystem.Current.LocalStorage;
        string FileName = "Settings.json";

        public async Task Read()
        {
            Items = null;
            try
            {
                if (await Folder.CheckExistsAsync(FileName) == ExistenceCheckResult.FileExists)
                {
                    var File = await Folder.GetFileAsync(FileName);
                    Items = JsonConvert.DeserializeObject<List<SettingValues>>(await File.ReadAllTextAsync());
                }
            }
            catch (Exception ex)
            {
                Items = null;
                Debug.WriteLine(ex.ToString());
            }
            if (Items == null || Items.Count == 0)
                LoadDefaults();

            ChangAndRebuild();
        }

        public async void Write()
        {
            try
            {
                var File = await Folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
                await File.WriteAllTextAsync(JsonConvert.SerializeObject(Items, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            ChangAndRebuild();
        }

        public void Default(SettingValues settingValues)
        {
            if (!settingValues.Default)
                return;
            foreach (var item in Items)
                if (item != settingValues)
                    item.Default = false;
        }

        public void LoadDefaults()
        {
            Items = new List<SettingValues>
            {
                new SettingValues
                {
                    Name = "Scan Only",
                    Default = false,
                    Deleteable = false,
                    RESTUrl = "",

                },
                new SettingValues
                {
                    Name = "Scan and REST localhost",
                    Default = false,
                    RESTUrl = "http://localhost:9876/Scan"
                },
                new SettingValues
                {
                    Name = "example.org",
                    Default = false,
                    RESTUrl = "http://example.org"
                },
                new SettingValues
                {
                    Name = "OCR Rowa",
                    Default = true,
                    RESTUrl = "http://lwdeu089kbdk32:14261/Master/Find"
                },
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


