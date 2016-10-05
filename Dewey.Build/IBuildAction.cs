using Dewey.Manifest.Component;
using System.Xml.Linq;

namespace Dewey.Build
{
    interface IBuildAction
    {
        string BuildType { get; }

        bool Build(ComponentManifest componentManifest, XElement buildElement);
    }
}
