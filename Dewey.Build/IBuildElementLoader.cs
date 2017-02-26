using System.Xml.Linq;

namespace Dewey.Build
{
    public interface IBuildElementLoader
    {
        void LoadFromComponentManifest(BuildCommand command, XElement componentElement);
    }
}