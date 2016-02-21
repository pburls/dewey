using System.Xml.Linq;

namespace Dewey.CLI.Deployments
{
    interface IDeploymentAction
    {
        void Deploy(RepositoryComponent repoComponent, ComponentManifest componentManifest,XElement deploymentElement);
    }
}
