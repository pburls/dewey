using Dewey.Manifest.Models;

namespace Dewey.Build
{
    public interface IBuildAction
    {
        string BuildType { get; }

        bool Build(Component componentManifest, Models.Build build);
    }
}
