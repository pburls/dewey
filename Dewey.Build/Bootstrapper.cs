using SimpleInjector;

namespace Dewey.Build
{
    public class Bootstrapper
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<IMSBuildProcess, MSBuildProcess>();
            container.Register<IBuildActionFactory, BuildActionFactory>();
            container.Register<IBuildElementLoader, BuildElementLoader>();

            container.RegisterCollection(typeof(IBuildAction), new[] { typeof(Bootstrapper).Assembly });

        }
    }
}
