using Dewey.Messaging;

namespace Dewey.Build.Events
{
    public class ComponentNotFoundResult : BuildCommandEventBase
    {
        public ComponentNotFoundResult(BuildCommand command) : base(command)
        {
        }
    }
}
