using Newtonsoft.Json;
using Commander.Entities;
using System.IO;

namespace Commander.Services
{
    public class ConfigService
    {
        public Config GetConfig()
        {
            var file = "Config.json";
            var data = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Config>(data);
        }
    }
}
