using Newtonsoft.Json.Linq;
using System;

namespace Dewey.Build.Models
{
    public class Build : IEquatable<Build>
    {
        public JObject BackingData { get; protected set; }
        public string type { get { return (string)BackingData["type"]; } set { BackingData["type"] = value; } }

        public Build()
        {
            BackingData = new JObject();
        }

        public Build(JObject data)
        {
            BackingData = data;
        }

        public string ToJson()
        {
            return BackingData.ToString();
        }

        public bool Equals(Build other)
        {
            if (other == null) return false;

            return type == other.type;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Build other = obj as Build;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode();
        }

        public static bool operator ==(Build a, Build b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.type == b.type;
        }

        public static bool operator !=(Build a, Build b)
        {
            return !(a == b);
        }
    }
}
