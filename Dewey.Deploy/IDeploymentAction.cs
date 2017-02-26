using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using System.Xml.Linq;

namespace Dewey.Deploy
{
    public interface IDeploymentAction
    {
        string Type { get; }

        bool Deploy(ComponentManifest componentManifest, XElement deploymentElement);
    }
}
