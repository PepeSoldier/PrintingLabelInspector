using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace XLIB_COMMON.Model
{
    public class CustomSettings
    {
        private string Path { get; set; }
        public string FileName { get; set; }
        //public List<CustomSettingsValue> Settings {get;set;}
        Dictionary<string, string> Settings = new Dictionary<string, string>();

        public CustomSettings(string fileName)
        {
            Path = AppDomain.CurrentDomain.BaseDirectory + "Configuration";
            FileName = fileName;
            Read();
        }

        public string GetValue(string settingName)
        {
            var s = Settings.FirstOrDefault(x => x.Key == settingName);
            return s.Value;
        }

        private void Read()
        {
            string[] lineElements;
            try
            {
                var lines = File.ReadAllLines(Path + "\\" + FileName);
                for (var i = 0; i < lines.Length; i++)
                {
                    lineElements = lines[i].Split(':');

                    try
                    {
                        if (lineElements.Length > 1)
                            Settings.Add(lineElements[0], lineElements[1]);
                    }
                    catch (ArgumentException)
                    {
                        //LoggerSingleton.Instance.SaveLog("An element with Key: " + lineElements[0] + " already exists.");
                        //return null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: " + e.Message);
            }
        }
    }

    //public class CustomSettingsValue
    //{
    //    public string Name { get; set; }
    //    public string Value { get; set; }
    //}

}