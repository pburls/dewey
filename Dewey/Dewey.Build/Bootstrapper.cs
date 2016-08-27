using SimpleInjector;

namespace Dewey.Build
{
    public class Bootstrapper
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<IMSBuildProcess, MSBuildProcess>(Lifestyle.Singleton);
        }
    }
}
