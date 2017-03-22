using Dewey.Manifest.Models;
using Newtonsoft.Json.Linq;

namespace Dewey.Build.Models
{
    public static class ComponentExtensions
    {
        public static bool IsBuildable(this Component component)
        {
            var build = component.BackingData["build"];
            return build != null && build is JObject;
        }
    }
}
