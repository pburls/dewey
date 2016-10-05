namespace Dewey.Deploy.Events
{
    public class DeployCommandStarted : DeployCommandEvent
    {
        public DeployCommandStarted(DeployCommand command) : base(command)
        {

        }
    }
}
