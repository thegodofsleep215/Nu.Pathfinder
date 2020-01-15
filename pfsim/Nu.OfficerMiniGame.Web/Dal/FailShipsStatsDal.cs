using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    public class FailShipsStatsDal : IShipStatsDal
    {
        private string folder;

        public FailShipsStatsDal(string rootDir)
        {
            this.folder = Path.Combine(rootDir, "Ships");
        }

        public List<string> GetNames()
        {
            if (!Directory.Exists(folder))
            {
                return new List<string>();
            }
            var files = Directory.GetFiles(folder, "*.json").ToList();

            return files.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
        }

        public ShipStats Get(string name)
        {
            if (!Directory.Exists(folder))
            {
                return null;
            }
            string filename;
            if (File.Exists(name))
            {
                filename = Path.GetFileName(name);
            }
            else
            {
                filename = $"{name.Replace(' ', '_')}.json";
            }
            string file = Directory.GetFiles(folder, filename).First();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            return JsonConvert.DeserializeObject<ShipStats>(File.ReadAllText(file), settings);
        }
    }
}
