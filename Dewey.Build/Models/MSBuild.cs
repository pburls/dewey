using System.Collections.Generic;

namespace Dewey.Build.Models
{
    public class MSBuild : Build
    {
        public string target { get { return (string)BackingData["target"]; } set { BackingData["target"] = value; } }
        public string msbuildVersion { get { return (string)BackingData["msbuildVersion"]; } set { BackingData["msbuildVersion"] = value; } }

        public MSBuild(Build build)
        {
            BackingData = build.BackingData;
        }

        public IEnumerable<string> GetMissingAttributes()
        {
            var missingAttList = new List<string>();

            if (string.IsNullOrWhiteSpace(target))
            {
                missingAttList.Add("target");
            }

            if (string.IsNullOrWhiteSpace(msbuildVersion))
            {
                missingAttList.Add("msbuildVersion");
            }

            return missingAttList;
        }
    }
}
