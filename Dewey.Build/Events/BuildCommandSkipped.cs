namespace Dewey.Build.Events
{
    public class BuildCommandSkipped : BuildCommandEvent
    {
        public BuildCommandSkipped(BuildCommand command) : base(command)
        {
        }
    }
}
