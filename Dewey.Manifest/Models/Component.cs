using Dewey.File;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Dewey.Manifest.Models
{
    public class Component : IEquatable<Component>
    {
        public JObject BackingData { get; private set; }

        public string name { get { return (string)BackingData["name"]; } set { BackingData["name"] = value; } }
        public string type { get { return (string)BackingData["type"]; } set { BackingData["type"] = value; } }
        public string subtype { get { return (string)BackingData["subtype"]; } set { BackingData["subtype"] = value; } }
        public string context { get { return (string)BackingData["context"]; } set { BackingData["context"] = value; } }

        public IManifestFileReader File { get; set; }

        public Component()
        {
            BackingData = new JObject();
        }

        public Component(JObject data)
        {
            BackingData = data;
        }

        public string ToJson()
        {
            return BackingData.ToString();
        }

        public static Component FromJson(string json)
        {
            return new Component(JObject.Parse(json));
        }

        public bool Equals(Component other)
        {
            if (other == null) return false;

            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Component other = obj as Component;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public static bool operator ==(Component a, Component b)
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

        public static bool operator !=(Component a, Component b)
        {
            return !(a == b);
        }
    }

    public class Dependency
    {
        public string type { get; set; }
        public string name { get; set; }
    }
}
