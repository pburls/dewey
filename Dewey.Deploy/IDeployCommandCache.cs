namespace Dewey.Deploy
{
    public interface IDeployCommandCache
    {
        bool IsComponentAlreadyDeployed(string component);
    }
}
