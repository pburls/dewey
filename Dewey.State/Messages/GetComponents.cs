using Dewey.Messaging;
using System.Collections.Generic;

namespace Dewey.State.Messages
{
    public class GetComponents : ICommand
    {
        public GetComponents()
        {

        }
    }

    public class GetComponentsResult : IEvent
    {
        public GetComponents Command { get; private set; }
        public IEnumerable<Component> Components { get; private set; }

        public GetComponentsResult(GetComponents command, IEnumerable<Component> components)
        {
            Command = command;
            Components = components;
        }
    }
}
