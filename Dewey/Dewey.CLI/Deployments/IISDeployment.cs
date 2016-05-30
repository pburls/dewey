using Dewey.Manifest.Component;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dewey.CLI.Deployments
{
    class IISDeployment : IDeploymentAction
    {
        public void Deploy(RepositoryComponent repoComponent, ComponentManifest componentManifest, XElement deploymentElement)
        {
            var iisDeploymentArgs = IISDeploumentArgs.ParseIISDeploymentElement(deploymentElement);
            string contentPath = Path.Combine(Environment.CurrentDirectory, repoComponent.Location, iisDeploymentArgs.Content);
            Console.WriteLine("IIS Deployment of site '{0}' for content path '{1}'.", iisDeploymentArgs.SiteName, contentPath);

            DirectoryInfo contentDirectoryInfo = new DirectoryInfo(contentPath);
            if (!contentDirectoryInfo.Exists)
            {
                Console.WriteLine("Content path '{0}' does not exist.", contentPath);
            }

            ServerManager serverManager = new ServerManager();

            var appPool = serverManager.ApplicationPools.FirstOrDefault(x => x.Name == iisDeploymentArgs.AppPool);
            if (appPool == null)
            {
                Console.WriteLine("Creating new IIS App Pool'{0}'.", iisDeploymentArgs.AppPool);
                appPool = serverManager.ApplicationPools.Add(iisDeploymentArgs.AppPool);
            }

            var site = serverManager.Sites.FirstOrDefault(x => x.Name == iisDeploymentArgs.SiteName);
            if (site == null)
            {
                Console.WriteLine("Creating new IIS Site '{0}' on port {2} mapped to path '{1}'.", iisDeploymentArgs.SiteName, contentDirectoryInfo.FullName, iisDeploymentArgs.Port);
                site = serverManager.Sites.Add(iisDeploymentArgs.SiteName, contentDirectoryInfo.FullName, iisDeploymentArgs.Port);
            }

            if (site.Applications[0].ApplicationPoolName != iisDeploymentArgs.AppPool)
            {
                Console.WriteLine("Setting IIS Site '{0}' to use App Pool '{1}'.", iisDeploymentArgs.SiteName, iisDeploymentArgs.AppPool);
                site.Applications[0].ApplicationPoolName = iisDeploymentArgs.AppPool;
            }

            if (site.Applications[0].VirtualDirectories[0].PhysicalPath != contentDirectoryInfo.FullName)
            {
                Console.WriteLine("Setting IIS Site '{0}' to use content '{1}'.", iisDeploymentArgs.SiteName, contentDirectoryInfo.FullName);
                site.Applications[0].VirtualDirectories[0].PhysicalPath = contentDirectoryInfo.FullName;
            }

            if (site.Bindings[0].EndPoint.Port != iisDeploymentArgs.Port)
            {
                Console.WriteLine("Setting IIS Site '{0}' to use port '{1}'.", iisDeploymentArgs.SiteName, iisDeploymentArgs.Port);
                site.Bindings[0].EndPoint.Port = iisDeploymentArgs.Port;
            }

            serverManager.CommitChanges();
        }

        class IISDeploumentArgs
        {
            public string SiteName { get; private set; }

            public string AppPool { get; private set; }

            public int Port { get; set; }

            public string Content { get; private set; }
            
            private IISDeploumentArgs(string siteName, string appPool, int port, string content)
            {
                SiteName = siteName;
                AppPool = appPool;
                Port = port;
                Content = content;
            }

            public static IISDeploumentArgs ParseIISDeploymentElement(XElement deploymentElement)
            {
                var siteNameAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "siteName");
                if (siteNameAtt == null || string.IsNullOrWhiteSpace(siteNameAtt.Value))
                {
                    throw new ArgumentException(string.Format("IIS Deployment element without a valid siteName: {0}", deploymentElement.ToString()), "deploymentElement");
                }

                var appPoolAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "appPool");
                if (appPoolAtt == null || string.IsNullOrWhiteSpace(appPoolAtt.Value))
                {
                    throw new ArgumentException(string.Format("IIS Deployment element without a valid appPool: {0}", deploymentElement.ToString()), "deploymentElement");
                }

                var portAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "port");
                int port;
                if (portAtt == null || string.IsNullOrWhiteSpace(portAtt.Value))
                {
                    throw new ArgumentException(string.Format("IIS Deployment element without a valid port: {0}", deploymentElement.ToString()), "deploymentElement");
                }
                else if (!int.TryParse(portAtt.Value, out port))
                {
                    throw new ArgumentException(string.Format("IIS Deployment element with invalid port: {0}", deploymentElement.ToString()), "deploymentElement");
                }

                var contentAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "content");
                if (contentAtt == null || string.IsNullOrWhiteSpace(contentAtt.Value))
                {
                    throw new ArgumentException(string.Format("IIS Deployment element without a valid content: {0}", deploymentElement.ToString()), "deploymentElement");
                }

                return new IISDeploumentArgs(siteNameAtt.Value, appPoolAtt.Value, port, contentAtt.Value);
            }
        }
    }
}
