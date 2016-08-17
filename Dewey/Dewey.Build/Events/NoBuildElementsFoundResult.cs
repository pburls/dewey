namespace Dewey.Build.Events
{
    public class NoBuildElementsFoundResult : BuildCommandEvent
    {
        public NoBuildElementsFoundResult(BuildCommand command) : base(command)
        {
        }
    }
}
