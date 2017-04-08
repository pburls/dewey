using Ark3.Event;

namespace Dewey.Build.Events
{
    public class BuildCommandStarted : BuildCommandEvent
    {
        public BuildCommandStarted(BuildCommand command) : base(command)
        {
        }
    }
}
