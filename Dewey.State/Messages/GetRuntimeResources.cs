using Dewey.Messaging;
using System.Collections.Generic;

namespace Dewey.State.Messages
{
    public class GetRuntimeResources : ICommand
    {

    }

    public class GetRuntimeResourcesResult : IEvent
    {
        public GetRuntimeResources Command { get; private set; }
        public IReadOnlyDictionary<string, RuntimeResource> RuntimeResources { get; private set; }

        public GetRuntimeResourcesResult(GetRuntimeResources command, IReadOnlyDictionary<string, RuntimeResource> runtimeResources)
        {
            Command = command;
            RuntimeResources = runtimeResources;
        }
    }
}
