using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;

namespace User.PluginSdkDemo.DTO
{
    internal class ProfileHolder
    {
        public int Release { get; set; }
        public GlobalSettings Global { get; set; }
        public List<Profile> Profiles { get; set; }

        public static ProfileHolder LoadFromJson(string profilePath)
        {
            try 
            {
                if (!File.Exists(profilePath))
                {
                    return null;
                }
                string text = File.ReadAllText(profilePath);
                return JsonSerializer.Deserialize<ProfileHolder>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    internal class GlobalSettings
        {
        public bool Donotnotify_updates { get; set; }
        public string Language { get; set; }
    }

    internal class Profile
    {
        public string Name { get; set; }
        public List<ProfileData> Data { get; set; }
    }

    internal class ProfileData
    {
        public string Fullname { get; set; }
        public string Cls { get; set; }
        public int Instance { get; set; }
        public string Ccmd { get; set; }
        public int Value { get; set; }
    }
}
