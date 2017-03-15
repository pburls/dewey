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
                return BackingData["manifestFiles"].Select(x => new ManifestFile(x as JObject)).ToArray();
            }
            set
            {
                BackingData["manifestFiles"] = new JArray(value.Select(x => x.BackingData));
            }
        }
        public Component[] components
        {
            get
            {
                return BackingData["components"].Select(x => new Component(x as JObject)).ToArray();
            }
            set
            {
                BackingData["components"] = new JArray(value.Select(x => x.BackingData));
            }
        }
        public RuntimeResource[] runtimeResources
        {
            get
            {
                return BackingData["runtimeResources"].Select(x => new RuntimeResource(x as JObject)).ToArray();
            }
            set
            {
                BackingData["runtimeResources"] = new JArray(value.Select(x => x.BackingData));
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
            return new Manifest(JObject.Parse(json));
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
