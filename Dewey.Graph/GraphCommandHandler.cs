using Dewey.Manifest.Dependency;
using Dewey.Messaging;
using Dewey.State.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Graph
{
    public class GraphCommandHandler :
        ICommandHandler<GraphCommand>,
        IEventHandler<GetComponentsResult>,
        IEventHandler<DependencyElementResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;

        readonly List<DependencyElementResult> _dependencies = new List<DependencyElementResult>();

        public GraphCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;

            eventAggregator.Subscribe<GetComponentsResult>(this);
            eventAggregator.Subscribe<DependencyElementResult>(this);
        }

        public void Execute(GraphCommand command)
        {
            _commandProcessor.Execute(new GetComponents());
        }

        public void Handle(GetComponentsResult getComponentsResult)
        {
            foreach (var component in getComponentsResult.Components)
            {
                DependencyElementResult.LoadDependencies(component.ComponentElement, component.ComponentManifest, _eventAggregator);
            }

            var graphStringBuilder = new StringBuilder();
            foreach (var dependecy in _dependencies)
            {
                graphStringBuilder.AppendLine(string.Format("g.addEdge('{0}', '{1}');", dependecy.Parent.Name, dependecy.Name));
            }
        }

        public void Handle(DependencyElementResult dependencyElementResult)
        {
            _dependencies.Add(dependencyElementResult);
        }
    }
}
