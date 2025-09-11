using System.Collections.Generic;
using User.PluginSdkDemo.Utils;

namespace User.PluginSdkDemo.DTO
{
    internal class ProfileHolder
    {
        public int Release { get; set; }
        public GlobalSettings Global { get; set; }
        public List<Profile> Profiles { get; set; }

        public static ProfileHolder LoadFromJson(string profilePath)
        {
            return JsonHandler.LoadFromJsonFile<ProfileHolder>(profilePath);
        }
    }

    internal class GlobalSettings
        {
        public bool DonotnotifyUpdates { get; set; }
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
        public string Cmd { get; set; }
        public int Value { get; set; }
    }
}
