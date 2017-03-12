using System;

namespace Dewey.Manifest.Models
{
    public class Component : IEquatable<Component>
    {
        public string name { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public string context { get; set; }
        public Build[] builds { get; set; }
        public Dependency[] dependencies { get; set; }

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

    public class Build
    {
        public string type { get; set; }
        public string target { get; set; }
        public string msbuildVersion { get; set; }
    }

    public class Dependency
    {
        public string type { get; set; }
        public string name { get; set; }
    }
}
