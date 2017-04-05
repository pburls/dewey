namespace Dewey.Deploy.Events
{
    public class DeployCommandSkipped : DeployCommandEvent
    {
        public DeployCommandSkipped(DeployCommand command) : base(command)
        {

        }
    }
}
