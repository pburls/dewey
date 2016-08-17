namespace Dewey.Build.Events
{
    public class NoBuildElementsFoundResult : BuildCommandEventBase
    {
        public NoBuildElementsFoundResult(BuildCommand command) : base(command)
        {
        }
    }
}
