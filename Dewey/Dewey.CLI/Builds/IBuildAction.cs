using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using System.Xml.Linq;

namespace Dewey.CLI.Builds
{
    interface IBuildAction
    {
        void Build(ComponentItem repoComponent, ComponentManifest componentManifest, XElement buildElement);
    }
}
