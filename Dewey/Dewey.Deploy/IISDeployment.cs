using Dewey.Deploy.Events;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Deploy
{
    class IISDeployment : IDeploymentAction
    {
        readonly IEventAggregator _eventAggregator;

        public const string DEPLOYMENT_TYPE = "iis";

        public IISDeployment(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Deploy(ComponentManifest componentManifest, XElement deploymentElement)
        {
            var iisDeploymentArgs = IISDeploumentArgs.ParseIISDeploymentElement(deploymentElement);
            if (iisDeploymentArgs.MissingAttributes.Any())
            {
                _eventAggregator.PublishEvent(new DeploymentElementMissingAttributeResult(componentManifest, DEPLOYMENT_TYPE, deploymentElement, iisDeploymentArgs.MissingAttributes));
                return;
            }

            if (iisDeploymentArgs.InvalidAttributes.Any())
            {
                _eventAggregator.PublishEvent(new DeploymentElementInvalidAttributeResult(componentManifest, DEPLOYMENT_TYPE, deploymentElement, iisDeploymentArgs.InvalidAttributes));
                return;
            }

            //string contentPath = Path.Combine(Environment.CurrentDirectory, repoComponent.RelativeLocation, iisDeploymentArgs.Content);
            string contentPath = Path.Combine(componentManifest.File.DirectoryName, iisDeploymentArgs.Content);

            if (!Directory.Exists(contentPath))
            {
                _eventAggregator.PublishEvent(new DeploymentActionContentNotFoundResult(componentManifest, DEPLOYMENT_TYPE, contentPath));
                return;
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
        }

        class IISDeploumentArgs
        {
            public string SiteName { get; private set; }

            public string AppPool { get; private set; }

            public int Port { get; set; }

            public string Content { get; private set; }

            public IEnumerable<string> MissingAttributes { get; private set; }

            public IEnumerable<string> InvalidAttributes { get; private set; }

            private IISDeploumentArgs(string siteName, string appPool, int port, string content, IEnumerable<string> missingAttributes, IEnumerable<string> invalidAttributes)
            {
                SiteName = siteName;
                AppPool = appPool;
                Port = port;
                Content = content;
                MissingAttributes = missingAttributes;
                InvalidAttributes = invalidAttributes;
            }

            public override string ToString()
            {
                return string.Format("IIS Deploument Args. SiteName: {0}, AppPool: {1}, Port: {2}, Content: {3}.", SiteName, AppPool, Port, Content);
            }

            public static IISDeploumentArgs ParseIISDeploymentElement(XElement deploymentElement)
            {
                var missingAttList = new List<string>();
                var invalidAttList = new List<string>();
                var siteNameAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "siteName");
                if (siteNameAtt == null || string.IsNullOrWhiteSpace(siteNameAtt.Value))
                {
                    missingAttList.Add("siteName");
                }

                var appPoolAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "appPool");
                if (appPoolAtt == null || string.IsNullOrWhiteSpace(appPoolAtt.Value))
                {
                    missingAttList.Add("appPool");
                }

                var portAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "port");
                int port = 0;
                if (portAtt == null || string.IsNullOrWhiteSpace(portAtt.Value))
                {
                    missingAttList.Add("port");
                }
                else if (!int.TryParse(portAtt.Value, out port))
                {
                    invalidAttList.Add("port");
                }

                var contentAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "content");
                if (contentAtt == null || string.IsNullOrWhiteSpace(contentAtt.Value))
                {
                    missingAttList.Add("content");
                }

                if (missingAttList.Any() || invalidAttList.Any())
                {
                    return new IISDeploumentArgs(null, null, port, null, missingAttList, invalidAttList);
                }

                return new IISDeploumentArgs(siteNameAtt.Value, appPoolAtt.Value, port, contentAtt.Value, missingAttList, invalidAttList);
            }
        }
    }
}
