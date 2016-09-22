using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Build
{
    public class MSBuildArgs
    {
        public string BuildTarget { get; private set; }

        public string MSBuildVersion { get; private set; }

        public IEnumerable<string> MissingAttributes { get; private set; }

        public MSBuildArgs(string buildTarget, string msbuildVersion, IEnumerable<string> missingAttributes)
        {
            BuildTarget = buildTarget;
            MSBuildVersion = msbuildVersion;
            MissingAttributes = missingAttributes;
        }

        public static MSBuildArgs ParseMSBuildElement(XElement buildElement)
        {
            var missingAttList = new List<string>();

            var buildTargetAtt = buildElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "target");
            if (buildTargetAtt == null || string.IsNullOrWhiteSpace(buildTargetAtt.Value))
            {
                missingAttList.Add("target");
            }

            var versionAtt = buildElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "msbuildVersion");
            if (versionAtt == null || string.IsNullOrWhiteSpace(versionAtt.Value))
            {
                missingAttList.Add("msbuildVersion");
            }

            if (missingAttList.Any())
            {
                return new MSBuildArgs(null, null, missingAttList);
            }

            return new MSBuildArgs(buildTargetAtt.Value, versionAtt.Value, missingAttList);
        }

        public override string ToString()
        {
            return string.Format("MS Build Args: BuildTarget: {0}, MSBuildVersion: {1}", BuildTarget, MSBuildVersion);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            MSBuildArgs other = obj as MSBuildArgs;
            if (other == null)
            {
                return false;
            }

            return BuildTarget == other.BuildTarget && MSBuildVersion == other.MSBuildVersion;
        }

        public override int GetHashCode()
        {
            return BuildTarget.GetHashCode() ^ MSBuildVersion.GetHashCode();
        }

        public static bool operator ==(MSBuildArgs a, MSBuildArgs b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.BuildTarget == b.BuildTarget && a.MSBuildVersion == b.MSBuildVersion;
        }

        public static bool operator !=(MSBuildArgs a, MSBuildArgs b)
        {
            return !(a == b);
        }
    }
}
