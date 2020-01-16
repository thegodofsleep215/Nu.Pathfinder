using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    public abstract class BaseJsonFileDal<T> where T : class
    {
        protected string folder;
        protected BaseJsonFileDal(string folder)
        {
            this.folder = folder;

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

        public T Get(string name)
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
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(file), settings);
        }

        public void Update(string name, T obj)
        {
            var filename = Path.Combine(folder, $"{name}.json");
            File.WriteAllText(filename, JsonConvert.SerializeObject(obj));
        }

    }
}
