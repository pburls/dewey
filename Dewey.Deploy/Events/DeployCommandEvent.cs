using Dewey.Messaging;

namespace Dewey.Deploy.Events
{
    public abstract class DeployCommandEvent : IEvent
    {
        public string ComponentName { get; protected set; }

        public DeployCommandEvent(DeployCommand command)
        {
            ComponentName = command.ComponentName;
        }
    }
}
