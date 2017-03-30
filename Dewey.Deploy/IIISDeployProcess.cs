using Dewey.Manifest.Models;

namespace Dewey.Deploy
{
    public interface IIISDeployProcess
    {
        void Deploy(Component componentManifest, Models.IISDeploy iisDeploy, string contentPath);
    }
}
