using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace OpenFFBoardPlugin.Utils
{
    internal class JsonHandler
    {

        public static T LoadFromJsonFile<T>(string profilePath) where T : class
        {
            try
            {
                if (!File.Exists(profilePath))
                {
                    return null;
                }
                JsonSerializerSettings serSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                return JsonConvert.DeserializeObject<T>(File.ReadAllText(profilePath), serSettings);
            }
            catch (Exception ex)
            {
                SimHub.Logging.Current.Error($"catch error loading json: {ex.Message}");
            }

            return null;
        }
    }
}
