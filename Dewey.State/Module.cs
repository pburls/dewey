using Dewey.Messaging;
using Dewey.State.Messages;

namespace Dewey.State
{
    public class Module : IModule
    {
        readonly Store _store;

        public Module(Store store)
        {
            _store = store;
        }
    }
}
