using Newtonsoft.Json.Linq;
using System;

namespace Dewey.Manifest.Models
{
    public class Dependency : IEquatable<Dependency>
    {
        public JObject BackingData { get; protected set; }

        public string name { get { return (string)BackingData["name"]; } set { BackingData["name"] = value; } }
        public string type { get { return (string)BackingData["type"]; } set { BackingData["type"] = value; } }

        public Dependency()
        {
            BackingData = new JObject();
        }

        public Dependency(JObject data)
        {
            BackingData = data;
        }

        public string ToJson()
        {
            return BackingData.ToString();
        }

        public bool Equals(Dependency other)
        {
            if (other == null) return false;

            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Dependency other = obj as Dependency;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public static bool operator ==(Dependency a, Dependency b)
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

        public static bool operator !=(Dependency a, Dependency b)
        {
            return !(a == b);
        }
    }
}
