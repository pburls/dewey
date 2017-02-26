using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repository
{
    public class ManifestFileItem
    {
        public string Name { get; private set; }
        public string RelativeLocation { get; private set; }
        public RepositoryManifest RepositoryManifest { get; private set; }

        public ManifestFileItem(string name, string relativeLocation, RepositoryManifest repositoryManifest)
        {
            Name = name;
            RepositoryManifest = repositoryManifest;
            RelativeLocation = relativeLocation;
        }
    }
}
