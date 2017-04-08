using Dewey.Graph.Writers;
using Ark3.Event;
using Ark3.Command;

namespace Dewey.Graph
{
    public class GraphCommandHandlerFactory : ICommandHandlerFactory<GraphCommand>
    {
        readonly IEventAggregator _eventAggregator;
        readonly ICommandProcessor _commandProcessor;
        readonly IGraphGenerator _graphGenerator;
        readonly IGraphWriterFactory _graphWriterFactory;

        public GraphCommandHandlerFactory(IEventAggregator eventAggregator, ICommandProcessor commandProcessor, IGraphGenerator graphGenerator, IGraphWriterFactory graphWriterFactory)
        {
            _eventAggregator = eventAggregator;
            _commandProcessor = commandProcessor;
            _graphGenerator = graphGenerator;
            _graphWriterFactory = graphWriterFactory;
        }

        public ICommandHandler<GraphCommand> CreateHandler()
        {
            return new GraphCommandHandler(_commandProcessor, _eventAggregator, _graphGenerator, _graphWriterFactory);
        }
    }
}
