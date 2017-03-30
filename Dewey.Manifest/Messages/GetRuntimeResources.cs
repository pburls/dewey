using Dewey.Messaging;
using System.Collections.Generic;

namespace Dewey.Manifest.Messages
{
    public class GetRuntimeResources : ICommand
    {

    }

    public class GetRuntimeResourcesResult : IEvent
    {
        public GetRuntimeResources Command { get; private set; }
        public IReadOnlyDictionary<string, Models.RuntimeResource> RuntimeResources { get; private set; }

        public GetRuntimeResourcesResult(GetRuntimeResources command, IReadOnlyDictionary<string, Models.RuntimeResource> runtimeResources)
        {
            Command = command;
            RuntimeResources = runtimeResources;
        }
    }
}
