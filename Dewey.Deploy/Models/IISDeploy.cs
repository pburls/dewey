using System.Collections.Generic;

namespace Dewey.Deploy.Models
{
    public class IISDeploy : Deploy
    {
        public int port { get { return (int)BackingData["port"]; } set { BackingData["port"] = value; } }
        public string siteName { get { return (string)BackingData["siteName"]; } set { BackingData["siteName"] = value; } }
        public string appPool { get { return (string)BackingData["appPool"]; } set { BackingData["appPool"] = value; } }
        public string content { get { return (string)BackingData["content"]; } set { BackingData["content"] = value; } }

        public IISDeploy(Deploy deploy)
        {
            BackingData = deploy.BackingData;
        }

        public IEnumerable<string> GetMissingAttributes()
        {
            var missingAttList = new List<string>();

            if (BackingData["port"] == null)
            {
                missingAttList.Add("port");
            }

            if (string.IsNullOrWhiteSpace(siteName))
            {
                missingAttList.Add("siteName");
            }

            if (string.IsNullOrWhiteSpace(appPool))
            {
                missingAttList.Add("appPool");
            }

            return missingAttList;
        }

        public IEnumerable<string> GetInvalidAttributes()
        {
            var attributeList = new List<string>();

            var portText = (string)BackingData["port"];
            int port;
            if (!int.TryParse(portText, out port))
            {
                attributeList.Add("port");
            }

            return attributeList;
        }
    }
}
