using Dewey.Graph.DOT;
using Dewey.Graph.Writers;
using Dewey.Manifest;
using Dewey.Messaging;
using Moq;
using SimpleInjector;
using System.Collections.Generic;
using Xunit;

namespace Dewey.Graph.Test
{
    public class GraphCommandTest
    {
        Mock<IGraphGenerator> mockIGraphGenerator;
        Mock<IGraphWriterFactory> mockIGraphWriterFactory;
        Mock<IGraphWriter> mockIGraphWriter;

        Container container;
        ICommandProcessor commandProcessor;

        public GraphCommandTest()
        {
            mockIGraphGenerator = new Mock<IGraphGenerator>();
            mockIGraphWriterFactory = new Mock<IGraphWriterFactory>();
            mockIGraphWriter = new Mock<IGraphWriter>();

            mockIGraphWriterFactory.Setup(x => x.CreateWriter(It.IsAny<GraphCommand>())).Returns(mockIGraphWriter.Object);

            container = new Container();
            Messaging.Bootstrapper.RegisterTypes(container);
            File.Bootstrapper.RegisterTypes(container);
            Manifest.Bootstrapper.RegisterTypes(container);
            State.Bootstrapper.RegisterTypes(container);

            container.RegisterSingleton(mockIGraphGenerator.Object);
            container.RegisterSingleton(mockIGraphWriterFactory.Object);

            var moduleCataloge = container.GetInstance<ModuleCatalogue>();
            moduleCataloge.Load<State.Module>();
            moduleCataloge.Load<Manifest.Module>();

            commandProcessor = container.GetInstance<ICommandProcessor>();

            commandProcessor.RegisterHandler<GraphCommand, GraphCommandHandler>();

            var writeResult = new WriteGraphResult(null, null);
            mockIGraphWriter.Setup(x => x.Write(It.IsAny<string>())).Returns(writeResult);
        }

        [Fact]
        public void GraphCommand_Should_GenerateDOTGraphText()
        {
            //Given
            var graphCommand = new GraphCommand(false);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(graphCommand);

            //Then
            mockIGraphGenerator.Verify(x => x.GenerateDOTGraph(It.IsAny<IEnumerable<Node>>(), It.IsAny<IEnumerable<Edge>>(), It.IsAny<IEnumerable<Cluster>>()), Times.Once());
        }

        [Fact]
        public void GraphCommand_Should_WriteGraph()
        {
            //Given
            var graphCommand = new GraphCommand(false);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(graphCommand);

            //Then
            mockIGraphWriter.Verify(x => x.Write(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GraphCommand_Should_DispatchResult()
        {
            //Given
            var graphCommand = new GraphCommand(false);

            GenerateGraphResult generateGraphResult = null;
            var mockGenerateGraphResultEventHandler = new Mock<IEventHandler<GenerateGraphResult>>();
            mockGenerateGraphResultEventHandler.Setup(x => x.Handle(It.IsAny<GenerateGraphResult>())).Callback<GenerateGraphResult>((result) => { generateGraphResult = result; });
            var eventAggregator = container.GetInstance<IEventAggregator>();
            eventAggregator.Subscribe(mockGenerateGraphResultEventHandler.Object);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(graphCommand);

            //Then
            Assert.NotNull(generateGraphResult);
        }
    }
}
