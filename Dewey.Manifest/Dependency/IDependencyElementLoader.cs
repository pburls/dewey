using Dewey.Manifest.Component;
using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public interface IDependencyElementLoader
    {
        void LoadFromComponentManifest(ComponentManifest componentMandifest, XElement componentElement);
    }
}