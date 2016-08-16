using Dewey.Messaging;

namespace Dewey.Build.Events
{
    public class BuildCommandStarted : BuildCommandEventBase
    {
        public BuildCommandStarted(string componentName) : base(componentName)
        {
        }
    }
}
