using Dewey.Messaging;
using Dewey.Manifest.Messages;
using System.Linq;

namespace Dewey.ListItems
{
    public class ListItemsCommandHandler :
        ICommandHandler<ListItemsCommand>,
        IEventHandler<GetComponentsResult>,
        IEventHandler<GetRuntimeResourcesResult>
    {
        readonly ICommandProcessor _commandProcessor;

        public ListItemsCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _commandProcessor = commandProcessor;

            eventAggregator.SubscribeAll(this);
        }

        public void Execute(ListItemsCommand command)
        {
            _commandProcessor.Execute(new GetComponents());
            _commandProcessor.Execute(new GetRuntimeResources());
        }

        public void Handle(GetComponentsResult getComponentsResult)
        {
            if (getComponentsResult.Components.Any())
            {
                foreach (var component in getComponentsResult.Components)
                {
                    component.Write();
                }
            }
        }

        public void Handle(GetRuntimeResourcesResult getRuntimeResourcesResult)
        {
            if (getRuntimeResourcesResult.RuntimeResources.Any())
            {
                foreach (var runtimeResource in getRuntimeResourcesResult.RuntimeResources.Values)
                {
                    runtimeResource.Write();
                }
            }
        }
    }
}
