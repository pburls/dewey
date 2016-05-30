using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using System.Xml.Linq;

namespace Dewey.CLI.Deployments
{
    interface IDeploymentAction
    {
        void Deploy(ComponentItem repoComponent, ComponentManifest componentManifest,XElement deploymentElement);
    }
}
