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
}
