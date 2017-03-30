using SimpleInjector;

namespace Dewey.Manifest
{
    public class Bootstrapper
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<Store>(Lifestyle.Singleton);
        }
    }
}
