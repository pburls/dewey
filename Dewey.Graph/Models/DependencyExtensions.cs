using Dewey.Manifest.Models;

namespace Dewey.Graph.Models
{
    public static class DependencyExtensions
    {
        public const string RUNTIME_RESOURCE_DEPENDENCY_TYPE = "runtimeResource";
        
        public static bool IsComponentDependency(this Dependency dependency)
        {
            return dependency.type == ComponentDependency.COMPONENT_DEPENDENCY_TYPE;
        }

        public static bool IsRuntimeResourceDependency(this Dependency dependency)
        {
            return dependency.type == RUNTIME_RESOURCE_DEPENDENCY_TYPE;
        }
    }
}
