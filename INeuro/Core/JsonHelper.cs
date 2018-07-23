using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace INeuro.Core
{
    class JsonHelper
    {
        public static void Serialize<T>(T obj, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(obj));
        }

        public static T Deserialize<T>(string path)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }

    }
}
