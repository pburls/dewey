using Dewey.Messaging;

namespace Dewey.Build.Events
{
    public class ComponentNotFoundResult : BuildCommandEventBase
    {
        public ComponentNotFoundResult(string componentName) : base(componentName)
        {
        }
    }
}
