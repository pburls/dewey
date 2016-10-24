using Dewey.Deploy.Events;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using Microsoft.Web.Administration;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Xml.Linq;

namespace Dewey.Deploy
{
    class IISDeployment : IDeploymentAction
    {
        readonly IEventAggregator _eventAggregator;

        public const string DEPLOYMENT_TYPE = "iis";

        public string Type { get { return DEPLOYMENT_TYPE; } }

        public IISDeployment(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public bool Deploy(ComponentManifest componentManifest, XElement deploymentElement)
        {
            var iisDeploymentArgs = IISDeploumentArgs.ParseIISDeploymentElement(deploymentElement);
            if (iisDeploymentArgs.MissingAttributes.Any())
            {
                _eventAggregator.PublishEvent(new DeploymentElementMissingAttributeResult(componentManifest, DEPLOYMENT_TYPE, deploymentElement, iisDeploymentArgs.MissingAttributes));
                return false;
            }

            if (iisDeploymentArgs.InvalidAttributes.Any())
            {
                _eventAggregator.PublishEvent(new DeploymentElementInvalidAttributeResult(componentManifest, DEPLOYMENT_TYPE, deploymentElement, iisDeploymentArgs.InvalidAttributes));
                return false;
            }

            string contentPath = Path.Combine(componentManifest.File.DirectoryName, iisDeploymentArgs.Content);

            if (!Directory.Exists(contentPath))
            {
                _eventAggregator.PublishEvent(new DeploymentActionContentNotFoundResult(componentManifest, DEPLOYMENT_TYPE, contentPath));
                return false;
            }

            if (!IsAdministrator())
            {
                _eventAggregator.PublishEvent(new DeploymentActionFailed(componentManifest, DEPLOYMENT_TYPE, "Administrator priviledges required. Please run as Administrator."));
                return false;
            }

            _eventAggregator.PublishEvent(new DeploymentActionStarted(componentManifest, DEPLOYMENT_TYPE, iisDeploymentArgs));

            ServerManager serverManager = new ServerManager();

            var appPool = serverManager.ApplicationPools.FirstOrDefault(x => x.Name == iisDeploymentArgs.AppPool);
            if (appPool == null)
            {
                _eventAggregator.PublishEvent(new DeploymentActionOutputMessage(componentManifest, DEPLOYMENT_TYPE, string.Format("Creating new IIS App Pool'{0}'.", iisDeploymentArgs.AppPool)));
                appPool = serverManager.ApplicationPools.Add(iisDeploymentArgs.AppPool);
            }

            var site = serverManager.Sites.FirstOrDefault(x => x.Name == iisDeploymentArgs.SiteName);
            if (site == null)
            {
                _eventAggregator.PublishEvent(new DeploymentActionOutputMessage(componentManifest, DEPLOYMENT_TYPE, string.Format("Creating new IIS Site '{0}' on port {2} mapped to path '{1}'.", iisDeploymentArgs.SiteName, contentPath, iisDeploymentArgs.Port)));
                site = serverManager.Sites.Add(iisDeploymentArgs.SiteName, contentPath, iisDeploymentArgs.Port);
            }

            if (site.Applications[0].ApplicationPoolName != iisDeploymentArgs.AppPool)
            {
                _eventAggregator.PublishEvent(new DeploymentActionOutputMessage(componentManifest, DEPLOYMENT_TYPE, string.Format("Setting IIS Site '{0}' to use App Pool '{1}'.", iisDeploymentArgs.SiteName, iisDeploymentArgs.AppPool)));
                site.Applications[0].ApplicationPoolName = iisDeploymentArgs.AppPool;
            }

            if (site.Applications[0].VirtualDirectories[0].PhysicalPath != contentPath)
            {
                _eventAggregator.PublishEvent(new DeploymentActionOutputMessage(componentManifest, DEPLOYMENT_TYPE, string.Format("Setting IIS Site '{0}' to use content '{1}'.", iisDeploymentArgs.SiteName, contentPath)));
                site.Applications[0].VirtualDirectories[0].PhysicalPath = contentPath;
            }

            if (site.Bindings[0].EndPoint.Port != iisDeploymentArgs.Port)
            {
                _eventAggregator.PublishEvent(new DeploymentActionOutputMessage(componentManifest, DEPLOYMENT_TYPE, string.Format("Setting IIS Site '{0}' to use port '{1}'.", iisDeploymentArgs.SiteName, iisDeploymentArgs.Port)));
                site.Bindings[0].EndPoint.Port = iisDeploymentArgs.Port;
            }

            serverManager.CommitChanges();

            _eventAggregator.PublishEvent(new DeploymentActionCompletedResult(componentManifest, DEPLOYMENT_TYPE, iisDeploymentArgs));

            return true;
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
