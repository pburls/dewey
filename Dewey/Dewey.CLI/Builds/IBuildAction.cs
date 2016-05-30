using Dewey.Manifest.Component;
using System.Xml.Linq;

namespace Dewey.CLI.Builds
{
    interface IBuildAction
    {
        void Build(RepositoryComponent repoComponent, ComponentManifest componentManifest, XElement buildElement);
    }
}
