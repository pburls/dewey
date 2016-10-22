using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public interface IDependencyElementLoader
    {
        void LoadFromComponentManifest(XElement componentElement);
    }
}