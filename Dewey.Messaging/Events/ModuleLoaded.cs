using Ark3.Event;

namespace Dewey.Messaging.Events
{
    public class ModuleLoaded : IEvent
    {
        public IModule Module { get; private set; }

        public ModuleLoaded(IModule module)
        {
            Module = module;
        }
    }
}
