namespace Dewey.Deploy.Events
{
    public class ComponentNotFoundResult : DeployCommandEvent
    {
        public ComponentNotFoundResult(DeployCommand command) : base(command)
        {
        }
    }
}
