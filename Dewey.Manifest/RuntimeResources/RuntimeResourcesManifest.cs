using Dewey.File;
using System.Collections.Generic;

namespace Dewey.Manifest.RuntimeResources
{
    public class RuntimeResourcesManifest
    {
        public string Name { get; private set; }

        public IManifestFileReader File { get; private set; }

        public IEnumerable<RuntimeResourceItem> RuntimeResourceItems { get; private set; }

        public RuntimeResourcesManifest(string name, IManifestFileReader file, IEnumerable<RuntimeResourceItem> runtimeResourceItems)
        {
            Name = name;
            File = file;
            RuntimeResourceItems = runtimeResourceItems;
        }

        public RuntimeResourcesManifest WithRuntimeResourceItems(IEnumerable<RuntimeResourceItem> runtimeResourceItems)
        {
            return new RuntimeResourcesManifest(Name, File, runtimeResourceItems);
        }
    }
}
