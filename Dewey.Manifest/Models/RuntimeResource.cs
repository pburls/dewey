using Dewey.File;
using System;

namespace Dewey.Manifest.Models
{
    public class RuntimeResource : IEquatable<RuntimeResource>
    {
        public string type { get; set; }
        public string name { get; set; }
        public string provider { get; set; }
        public string format { get; set; }
        public string context { get; set; }
        public IManifestFileReader File { get; set; }

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
