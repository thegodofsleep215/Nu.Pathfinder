using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Nu.OfficerMiniGame.Dal.Dal
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
            string filename = $"{name}.json";
            if (!File.Exists(Path.Combine(folder, filename)))
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

        public bool Create(string name, T obj)
        {
            var filename = Path.Combine(folder, $"{name}.json");
            if (File.Exists(filename)) return false;
            File.WriteAllText(filename, JsonConvert.SerializeObject(obj));
            return true;
        }

        public void Delete(string name)
        {
            var filename = Path.Combine(folder, $"{name}.json");
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }
        
        public bool Exists(string name)
        {
            var filename = Path.Combine(folder, $"{name}.json");
            return File.Exists(filename);
        }
    }
}
