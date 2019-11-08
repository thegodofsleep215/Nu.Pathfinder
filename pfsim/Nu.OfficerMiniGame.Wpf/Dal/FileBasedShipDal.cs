using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nu.OfficerMiniGame.Wpf
{
    public class FileBasedShipDal : IShipDal
    {
        public List<Ship> GetAll()
        {
            var folder = ".\\Ships";
            if (!Directory.Exists(folder))
            {
                return new List<Ship>();
            }
            var files = Directory.GetFiles(folder, "*.json").ToList();


            var ships = new List<Ship>();
            files.ForEach(x =>
            {
                var ship = Get(x);
                if (ship != null)
                {
                    ships.Add(ship);
                }
            });
            return ships;
        }

        public Ship Get(string name)
        {
            var folder = ".\\Ships";
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
            return JsonConvert.DeserializeObject<Ship>(File.ReadAllText(file), settings);
        }
    }
}

