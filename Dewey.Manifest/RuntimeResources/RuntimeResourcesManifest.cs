using Dewey.File;

namespace Dewey.Manifest.RuntimeResources
{
    public class RuntimeResourcesManifest
    {
        public string Name { get; private set; }

        public IManifestFileReader File { get; private set; }

        public RuntimeResourcesManifest(string name, IManifestFileReader file)
        {
            Name = name;
            File = file;
        }
    }
}
