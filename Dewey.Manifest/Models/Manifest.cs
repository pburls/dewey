using Newtonsoft.Json.Linq;
using System.Linq;

namespace Dewey.Manifest.Models
{
    public class Manifest
    {
        public JObject BackingData { get; private set; }

        public ManifestFile[] manifestFiles
        {
            get
            {
                return BackingData["manifestFiles"]?.Select(x => new ManifestFile(x as JObject)).ToArray();
            }
            set
            {
                if (value != null)
                {
                    BackingData["manifestFiles"] = new JArray(value.Select(x => x.BackingData));
                }
                else
                {
                    BackingData.Remove("manifestFiles");
                }
            }
        }
        public Component[] components
        {
            get
            {
                return BackingData["components"]?.Select(x => new Component(x as JObject)).ToArray();
            }
            set
            {
                if (value != null)
                {
                    BackingData["components"] = new JArray(value.Select(x => x.BackingData));
                }
                else
                {
                    BackingData.Remove("components");
                }
            }
        }
        public RuntimeResource[] runtimeResources
        {
            get
            {
                return BackingData["runtimeResources"]?.Select(x => new RuntimeResource(x as JObject)).ToArray();
            }
            set
            {
                if (value != null)
                {
                    BackingData["runtimeResources"] = new JArray(value.Select(x => x.BackingData));
                }
                else
                {
                    BackingData.Remove("runtimeResources");
                }
            }
        }

        public Manifest()
        {
            BackingData = new JObject();
        }

        public Manifest(JObject data)
        {
            BackingData = data;
        }

        public string ToJson()
        {
            return BackingData.ToString();
        }

        public static Manifest FromJson(string json)
        {
            var data = JObject.Parse(json);
            return new Manifest(data);
        }
    }

    public class ManifestFile
    {
        public JObject BackingData { get; private set; }

        public string name { get { return (string)BackingData["name"]; } set { BackingData["name"] = value; } }
        public string location { get { return (string)BackingData["location"]; } set { BackingData["location"] = value; } }

        public ManifestFile()
        {
            BackingData = new JObject();
        }

        public ManifestFile(JObject data)
        {
            BackingData = data;
        }

        public string ToJsonString()
        {
            return BackingData.ToString();
        }

        public static ManifestFile FromJson(string json)
        {
            return new ManifestFile(JObject.Parse(json));
        }
    }
}
