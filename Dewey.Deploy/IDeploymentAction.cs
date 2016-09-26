using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using System.Xml.Linq;

namespace Dewey.Deploy
{
    interface IDeploymentAction
    {
        bool Deploy(ComponentManifest componentManifest, XElement deploymentElement);
    }
}
