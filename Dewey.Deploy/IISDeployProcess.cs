using System.Linq;
using Dewey.Deploy.Models;
using Microsoft.Web.Administration;
using Dewey.Deploy.Events;
using Dewey.Manifest.Models;
using Ark3.Event;

namespace Dewey.Deploy
{
    public class IISDeployProcess : IIISDeployProcess
    {
        readonly IEventAggregator _eventAggregator;

        public IISDeployProcess(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Deploy(Component componentManifest, IISDeploy iisDeploy, string contentPath)
        {
            ServerManager serverManager = new ServerManager();

            var appPool = serverManager.ApplicationPools.FirstOrDefault(x => x.Name == iisDeploy.appPool);
            if (appPool == null)
            {
                _eventAggregator.PublishEvent(new JsonDeploymentActionOutputMessage(componentManifest, iisDeploy, string.Format("Creating new IIS App Pool'{0}'.", iisDeploy.appPool)));
                appPool = serverManager.ApplicationPools.Add(iisDeploy.appPool);
            }

            var site = serverManager.Sites.FirstOrDefault(x => x.Name == iisDeploy.siteName);
            if (site == null)
            {
                _eventAggregator.PublishEvent(new JsonDeploymentActionOutputMessage(componentManifest, iisDeploy, string.Format("Creating new IIS Site '{0}' on port {2} mapped to path '{1}'.", iisDeploy.siteName, contentPath, iisDeploy.port)));
                site = serverManager.Sites.Add(iisDeploy.siteName, contentPath, iisDeploy.port);
            }

            if (site.Bindings[0].EndPoint.Port != iisDeploy.port)
            {
                _eventAggregator.PublishEvent(new JsonDeploymentActionOutputMessage(componentManifest, iisDeploy, string.Format("Recreating IIS Site '{0}' on port {2} mapped to path '{1}'.", iisDeploy.siteName, contentPath, iisDeploy.port)));
                serverManager.Sites.Remove(site);
                site = serverManager.Sites.Add(iisDeploy.siteName, contentPath, iisDeploy.port);
            }
            else
            {
                if (site.Applications[0].VirtualDirectories[0].PhysicalPath != contentPath)
                {
                    _eventAggregator.PublishEvent(new JsonDeploymentActionOutputMessage(componentManifest, iisDeploy, string.Format("Setting IIS Site '{0}' to use content '{1}'.", iisDeploy.siteName, contentPath)));
                    site.Applications[0].VirtualDirectories[0].PhysicalPath = contentPath;
                }
            }

            if (site.Applications[0].ApplicationPoolName != iisDeploy.appPool)
            {
                _eventAggregator.PublishEvent(new JsonDeploymentActionOutputMessage(componentManifest, iisDeploy, string.Format("Setting IIS Site '{0}' to use App Pool '{1}'.", iisDeploy.siteName, iisDeploy.appPool)));
                site.Applications[0].ApplicationPoolName = iisDeploy.appPool;
            }

            serverManager.CommitChanges();
        }
    }
}
