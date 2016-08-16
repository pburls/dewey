using Dewey.Messaging;

namespace Dewey.Build.Events
{
    public abstract class BuildCommandEventBase : IEvent
    {
        public string ComponentName { get; protected set; }

        public BuildCommandEventBase(string componentName)
        {
            ComponentName = componentName;
        }
    }
}
