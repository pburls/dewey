using SimpleInjector;

namespace Dewey.State
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<Store>(Lifestyle.Singleton);
        }
    }
}
