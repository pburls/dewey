using System.Collections.Generic;

namespace Dewey.Build.Models
{
    public class MSBuild : Build
    {
        public string target { get { return (string)BackingData[nameof(target)]; } set { BackingData[nameof(target)] = value; } }
        public string msbuildVersion { get { return (string)BackingData[nameof(msbuildVersion)]; } set { BackingData[nameof(msbuildVersion)] = value; } }

        public MSBuild(Build build)
        {
            BackingData = build.BackingData;
        }

        public IEnumerable<string> GetMissingAttributes()
        {
            var missingAttList = new List<string>();

            if (string.IsNullOrWhiteSpace(target))
            {
                missingAttList.Add(nameof(target));
            }

            if (string.IsNullOrWhiteSpace(msbuildVersion))
            {
                missingAttList.Add(nameof(msbuildVersion));
            }

            return missingAttList;
        }
    }
}
