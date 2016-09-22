using Dewey.Manifest.Component;
using Dewey.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Build.Events
{
    public class MSBuildExecutableNotFoundResult : IEvent
    {
        public ComponentManifest ComponentManifest { get; private set; }

        public string MSBuildVersion { get; private set; }

        public MSBuildExecutableNotFoundResult(ComponentManifest componentManifest, string msbuildVersion)
        {
            ComponentManifest = componentManifest;
            MSBuildVersion = msbuildVersion;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            MSBuildExecutableNotFoundResult other = obj as MSBuildExecutableNotFoundResult;
            if (other == null)
            {
                return false;
            }

            return ComponentManifest == other.ComponentManifest && MSBuildVersion == other.MSBuildVersion;
        }

        public override int GetHashCode()
        {
            return ComponentManifest.GetHashCode() ^ MSBuildVersion.GetHashCode();
        }

        public static bool operator ==(MSBuildExecutableNotFoundResult a, MSBuildExecutableNotFoundResult b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentManifest == b.ComponentManifest && a.MSBuildVersion == b.MSBuildVersion;
        }

        public static bool operator !=(MSBuildExecutableNotFoundResult a, MSBuildExecutableNotFoundResult b)
        {
            return !(a == b);
        }
    }
}
