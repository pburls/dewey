namespace Dewey.Build.Events
{
    public class NoBuildElementsFoundResult : BuildCommandEventBase
    {
        public NoBuildElementsFoundResult(string componentName) : base(componentName)
        {
        }
    }
}
