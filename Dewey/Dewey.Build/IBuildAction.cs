using Dewey.Manifest.Component;
using System.Xml.Linq;

namespace Dewey.Build
{
    interface IBuildAction
    {
        void Build(ComponentManifest componentManifest, XElement buildElement);
    }
}
