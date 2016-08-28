using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using System.Xml.Linq;

namespace Dewey.Deploy
{
    interface IDeploymentAction
    {
        void Deploy(ComponentManifest componentManifest, XElement deploymentElement);
    }
}
