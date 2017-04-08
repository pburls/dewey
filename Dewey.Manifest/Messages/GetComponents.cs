using Ark3.Command;
using Ark3.Event;
using System.Collections.Generic;

namespace Dewey.Manifest.Messages
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
        public IEnumerable<Models.Component> Components { get; private set; }

        public GetComponentsResult(GetComponents command, IEnumerable<Models.Component> components)
        {
            Command = command;
            Components = components;
        }
    }
}
