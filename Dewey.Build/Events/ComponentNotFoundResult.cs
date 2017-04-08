using Ark3.Event;

namespace Dewey.Build.Events
{
    public class ComponentNotFoundResult : BuildCommandEvent
    {
        public ComponentNotFoundResult(BuildCommand command) : base(command)
        {
        }
    }
}
