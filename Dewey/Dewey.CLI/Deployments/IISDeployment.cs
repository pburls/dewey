using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dewey.CLI.Deployments
{
    class IISDeployment : IDeploymentAction
    {
        string appcmdPath = @"C:\Windows\System32\inetsrv\appcmd.exe";

        public void Deploy(RepositoryComponent repoComponent, ComponentManifest componentManifest, XElement deploymentElement)
        {
            var iisDeploymentArgs = IISDeploumentArgs.ParseIISDeploymentElement(deploymentElement);

            //check for app pool
            var appPoolExists = AppPoolExists(iisDeploymentArgs.AppPool);

            //create app pool

            //check for siteId
            //remove siteId

            //check for port
            //fail on port

            //create site
        }

        bool AppPoolExists(string appPoolName)
        {
            var args = "list apppool";

            var msBuildStartInfo = new ProcessStartInfo(appcmdPath, args);
            msBuildStartInfo.UseShellExecute = false;
            var msBuildProcess = Process.Start(msBuildStartInfo);

            msBuildProcess.WaitForExit();

            return false;
        }

        class IISDeploumentArgs
        {
            public string SiteId { get; private set; }

            public string SiteName { get; private set; }

            public string AppPool { get; private set; }

            public string Port { get; set; }

            public string Content { get; private set; }
            
            public IISDeploumentArgs(string siteId, string siteName, string appPool, string port, string content)
            {
                SiteId = siteId;
                SiteName = siteName;
                AppPool = appPool;
                Port = port;
                Content = content;
            }

            public static IISDeploumentArgs ParseIISDeploymentElement(XElement deploymentElement)
            {
                var siteIdAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "siteId");
                if (siteIdAtt == null || string.IsNullOrWhiteSpace(siteIdAtt.Value))
                {
                    throw new ArgumentException(string.Format("IIS Deployment element without a valid siteId: {0}", deploymentElement.ToString()), "deploymentElement");
                }

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
                if (portAtt == null || string.IsNullOrWhiteSpace(portAtt.Value))
                {
                    throw new ArgumentException(string.Format("IIS Deployment element without a valid port: {0}", deploymentElement.ToString()), "deploymentElement");
                }

                var contentAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "content");
                if (contentAtt == null || string.IsNullOrWhiteSpace(contentAtt.Value))
                {
                    throw new ArgumentException(string.Format("IIS Deployment element without a valid content: {0}", deploymentElement.ToString()), "deploymentElement");
                }

                return new IISDeploumentArgs(siteIdAtt.Value, siteNameAtt.Value, appPoolAtt.Value, portAtt.Value, contentAtt.Value);
            }
        }
    }
}
