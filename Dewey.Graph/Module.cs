﻿using Dewey.Graph.Writers;
using Dewey.Messaging;
using Ark3.Event;
using Ark3.Command;

namespace Dewey.Graph
{
    public class Module : IModule
    {
        readonly GraphCommandWriter _writer;

        public Module(IEventAggregator eventAggregator, ICommandProcessor commandProcessor, IGraphGenerator graphGenerator, IGraphWriterFactory graphWriterFactory)
        {
            _writer = new GraphCommandWriter(eventAggregator);

            var graphCommandHandlerFactory = new GraphCommandHandlerFactory(eventAggregator, commandProcessor, graphGenerator, graphWriterFactory);
            commandProcessor.RegisterHandlerFactory(graphCommandHandlerFactory);
        }
    }
}
