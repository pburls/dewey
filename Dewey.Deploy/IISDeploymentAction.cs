using Dewey.Deploy.Events;
using Dewey.Messaging;
using System.IO;
using System.Linq;
using Dewey.Deploy.Models;
using Dewey.Manifest.Models;
using Dewey.File;

namespace Dewey.Deploy
{
    public class IISDeploymentAction : IDeploymentAction
    {
        readonly IEventAggregator _eventAggregator;
        readonly IFileService _fileService;
        readonly IIISDeployProcess _iisDeployProcess;
        readonly IUserService _userService;

        public const string DEPLOYMENT_TYPE = "iis";

        public string Type { get { return DEPLOYMENT_TYPE; } }

        public IISDeploymentAction(IEventAggregator eventAggregator, IFileService fileService, IIISDeployProcess iisDeployProccess, IUserService userService)
        {
            _eventAggregator = eventAggregator;
            _fileService = fileService;
            _iisDeployProcess = iisDeployProccess;
            _userService = userService;
        }

        public bool Deploy(Component componentManifest, Models.Deploy deploy)
        {
            var iisDeploy = new IISDeploy(deploy);
            var missingAttributes = iisDeploy.GetMissingAttributes();
            if (missingAttributes.Any())
            {
                _eventAggregator.PublishEvent(new JsonDeploymentMissingAttributesResult(componentManifest, iisDeploy, missingAttributes));
                return false;
            }

            var invalidAttributes = iisDeploy.GetInvalidAttributes();
            if (invalidAttributes.Any())
            {
                _eventAggregator.PublishEvent(new JsonDeploymentInvalidAttributeResult(componentManifest, iisDeploy, invalidAttributes));
                return false;
            }

            string contentPath = string.IsNullOrWhiteSpace(iisDeploy.content) ? componentManifest.File.DirectoryName : Path.Combine(componentManifest.File.DirectoryName, iisDeploy.content);
            if (!_fileService.DirectoryExists(contentPath))
            {
                _eventAggregator.PublishEvent(new JsonDeploymentActionContentNotFoundResult(componentManifest, iisDeploy, contentPath));
                return false;
            }

            if (!_userService.IsUserAdministrator())
            {
                _eventAggregator.PublishEvent(new JsonDeploymentActionFailed(componentManifest, iisDeploy, "Administrator priviledges required. Please run as Administrator."));
                return false;
            }

            _eventAggregator.PublishEvent(new JsonDeploymentActionStarted(componentManifest, iisDeploy));

            _iisDeployProcess.Deploy(componentManifest, iisDeploy, contentPath);

            _eventAggregator.PublishEvent(new JsonDeploymentActionCompletedResult(componentManifest, iisDeploy));

            return true;
        }

        //public bool Deploy(ComponentManifest componentManifest, XElement deploymentElement)
        //{
        //    var iisDeploymentArgs = IISDeploumentArgs.ParseIISDeploymentElement(deploymentElement);
        //    if (iisDeploymentArgs.MissingAttributes.Any())
        //    {
        //        _eventAggregator.PublishEvent(new DeploymentElementMissingAttributeResult(componentManifest, DEPLOYMENT_TYPE, deploymentElement, iisDeploymentArgs.MissingAttributes));
        //        return false;
        //    }

        //    if (iisDeploymentArgs.InvalidAttributes.Any())
        //    {
        //        _eventAggregator.PublishEvent(new DeploymentElementInvalidAttributeResult(componentManifest, DEPLOYMENT_TYPE, deploymentElement, iisDeploymentArgs.InvalidAttributes));
        //        return false;
        //    }

        //    string contentPath = string.IsNullOrWhiteSpace(iisDeploymentArgs.Content) ? componentManifest.File.DirectoryName : Path.Combine(componentManifest.File.DirectoryName, iisDeploymentArgs.Content);

        //    if (!Directory.Exists(contentPath))
        //    {
        //        _eventAggregator.PublishEvent(new DeploymentActionContentNotFoundResult(componentManifest, DEPLOYMENT_TYPE, contentPath));
        //        return false;
        //    }

        //    if (!IsAdministrator())
        //    {
        //        _eventAggregator.PublishEvent(new DeploymentActionFailed(componentManifest, DEPLOYMENT_TYPE, "Administrator priviledges required. Please run as Administrator."));
        //        return false;
        //    }

        //    _eventAggregator.PublishEvent(new DeploymentActionStarted(componentManifest, DEPLOYMENT_TYPE, iisDeploymentArgs));

        //    ServerManager serverManager = new ServerManager();

        //    var appPool = serverManager.ApplicationPools.FirstOrDefault(x => x.Name == iisDeploymentArgs.AppPool);
        //    if (appPool == null)
        //    {
        //        _eventAggregator.PublishEvent(new DeploymentActionOutputMessage(componentManifest, DEPLOYMENT_TYPE, string.Format("Creating new IIS App Pool'{0}'.", iisDeploymentArgs.AppPool)));
        //        appPool = serverManager.ApplicationPools.Add(iisDeploymentArgs.AppPool);
        //    }

        //    var site = serverManager.Sites.FirstOrDefault(x => x.Name == iisDeploymentArgs.SiteName);
        //    if (site == null)
        //    {
        //        _eventAggregator.PublishEvent(new DeploymentActionOutputMessage(componentManifest, DEPLOYMENT_TYPE, string.Format("Creating new IIS Site '{0}' on port {2} mapped to path '{1}'.", iisDeploymentArgs.SiteName, contentPath, iisDeploymentArgs.Port)));
        //        site = serverManager.Sites.Add(iisDeploymentArgs.SiteName, contentPath, iisDeploymentArgs.Port);
        //    }

        //    if (site.Bindings[0].EndPoint.Port != iisDeploymentArgs.Port)
        //    {
        //        _eventAggregator.PublishEvent(new DeploymentActionOutputMessage(componentManifest, DEPLOYMENT_TYPE, string.Format("Recreating IIS Site '{0}' on port {2} mapped to path '{1}'.", iisDeploymentArgs.SiteName, contentPath, iisDeploymentArgs.Port)));
        //        serverManager.Sites.Remove(site);
        //        site = serverManager.Sites.Add(iisDeploymentArgs.SiteName, contentPath, iisDeploymentArgs.Port);
        //    }
        //    else
        //    {
        //        if (site.Applications[0].VirtualDirectories[0].PhysicalPath != contentPath)
        //        {
        //            _eventAggregator.PublishEvent(new DeploymentActionOutputMessage(componentManifest, DEPLOYMENT_TYPE, string.Format("Setting IIS Site '{0}' to use content '{1}'.", iisDeploymentArgs.SiteName, contentPath)));
        //            site.Applications[0].VirtualDirectories[0].PhysicalPath = contentPath;
        //        }
        //    }

        //    if (site.Applications[0].ApplicationPoolName != iisDeploymentArgs.AppPool)
        //    {
        //        _eventAggregator.PublishEvent(new DeploymentActionOutputMessage(componentManifest, DEPLOYMENT_TYPE, string.Format("Setting IIS Site '{0}' to use App Pool '{1}'.", iisDeploymentArgs.SiteName, iisDeploymentArgs.AppPool)));
        //        site.Applications[0].ApplicationPoolName = iisDeploymentArgs.AppPool;
        //    }

        //    serverManager.CommitChanges();

        //    _eventAggregator.PublishEvent(new DeploymentActionCompletedResult(componentManifest, DEPLOYMENT_TYPE, iisDeploymentArgs));

        //    return true;
        //}

        //private static bool IsAdministrator()
        //{
        //    WindowsIdentity identity = WindowsIdentity.GetCurrent();
        //    WindowsPrincipal principal = new WindowsPrincipal(identity);
        //    return principal.IsInRole(WindowsBuiltInRole.Administrator);
        //}
    }
}
