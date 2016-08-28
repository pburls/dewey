using Dewey.Messaging;

namespace Dewey.Build.Events
{
    public class ComponentNotFoundResult : BuildCommandEvent
    {
        public ComponentNotFoundResult(BuildCommand command) : base(command)
        {
        }
    }
}
