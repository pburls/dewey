using Ark3.Command;
using Ark3.Event;
using SimpleInjector;

namespace Dewey.Messaging
{
    public class Bootstrapper
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<IEventAggregator, EventAggregator>(Lifestyle.Singleton);
            container.Register<ICommandProcessor, CommandProcessor>(Lifestyle.Singleton);
        }
    }
}
