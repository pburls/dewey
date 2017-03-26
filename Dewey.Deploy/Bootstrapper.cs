using SimpleInjector;

namespace Dewey.Deploy
{
    public class Bootstrapper
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<IDeploymentActionFactory, DeploymentActionFactory>();
            container.Register<IIISDeployProcess, IISDeployProcess>();

            container.RegisterCollection(typeof(IDeploymentAction), new[] { typeof(Bootstrapper).Assembly });
        }
    }
}
