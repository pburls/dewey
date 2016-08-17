using Dewey.Build.Events;
using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using System.Xml.Linq;

namespace Dewey.Build
{
    interface IBuildAction
    {
        void Build(ComponentManifest componentManifest, XElement buildElement);
    }
}
