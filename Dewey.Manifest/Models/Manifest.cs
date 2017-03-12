using System;

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

    public class RuntimeResource
    {
        public string type { get; set; }
        public string name { get; set; }
        public string provider { get; set; }
        public string format { get; set; }
        public string context { get; set; }
    }

}
