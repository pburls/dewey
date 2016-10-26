using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Deploy
{
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
            if (contentAtt == null || contentAtt.Value == null)
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
