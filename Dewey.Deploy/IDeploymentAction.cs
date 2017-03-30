using Dewey.Manifest.Models;

namespace Dewey.Deploy
{
    public interface IDeploymentAction
    {
        string Type { get; }

        bool Deploy(Component componentManifest, Models.Deploy deploy);
    }
}
