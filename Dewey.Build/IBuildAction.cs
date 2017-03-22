using Dewey.Manifest.Component;
using Dewey.Manifest.Models;
using System.Xml.Linq;

namespace Dewey.Build
{
    public interface IBuildAction
    {
        string BuildType { get; }

        bool Build(Component componentManifest, Models.Build build);
    }
}
