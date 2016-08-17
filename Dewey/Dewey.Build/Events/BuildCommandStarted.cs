using Dewey.Messaging;

namespace Dewey.Build.Events
{
    public class BuildCommandStarted : BuildCommandEventBase
    {
        public BuildCommandStarted(BuildCommand command) : base(command)
        {
        }
    }
}
