namespace Dewey.Manifest.Models
{
    public class Manifest
    {
        public Manifestfile[] manifestFiles { get; set; }
        public Component[] components { get; set; }
        public RuntimeResource[] runtimeResources { get; set; }
    }

    public class Manifestfile
    {
        public string name { get; set; }
        public string location { get; set; }
    }

    public class Component
    {
        public string name { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public string context { get; set; }
        public Build[] builds { get; set; }
        public Dependency[] dependencies { get; set; }
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

    public class RuntimeResource
    {
        public string type { get; set; }
        public string name { get; set; }
        public string provider { get; set; }
        public string format { get; set; }
        public string context { get; set; }
    }

}
