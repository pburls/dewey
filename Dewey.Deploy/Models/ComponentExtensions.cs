using Dewey.Manifest.Models;
using Newtonsoft.Json.Linq;

namespace Dewey.Deploy.Models
{
    public static class ComponentExtensions
    {
        public static bool IsDeployable(this Component component)
        {
            var deploy = component.BackingData["deploy"];
            return deploy != null && deploy is JObject;
        }
    }
}
