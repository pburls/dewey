using Dewey.File;
using Newtonsoft.Json.Linq;
using System;

namespace Dewey.Manifest.Models
{
    public class RuntimeResource : IEquatable<RuntimeResource>
    {
        public JObject BackingData { get; private set; }

        public string type { get { return (string)BackingData["type"]; } set { BackingData["type"] = value; } }
        public string name { get { return (string)BackingData["name"]; } set { BackingData["name"] = value; } }
        public string provider { get { return (string)BackingData["provider"]; } set { BackingData["provider"] = value; } }
        public string format { get { return (string)BackingData["format"]; } set { BackingData["format"] = value; } }
        public string context { get { return (string)BackingData["context"]; } set { BackingData["context"] = value; } }
        public IManifestFileReader File { get; set; }

        public RuntimeResource()
        {
            BackingData = new JObject();
        }

        public RuntimeResource(JObject data)
        {
            BackingData = data;
        }

        public string ToJson()
        {
            return BackingData.ToString();
        }

        public static RuntimeResource FromJson(string json)
        {
            return new RuntimeResource(JObject.Parse(json));
        }

        public bool Equals(RuntimeResource other)
        {
            if (other == null) return false;

            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            RuntimeResource other = obj as RuntimeResource;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public static bool operator ==(RuntimeResource a, RuntimeResource b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.name == b.name;
        }

        public static bool operator !=(RuntimeResource a, RuntimeResource b)
        {
            return !(a == b);
        }
    }
}
