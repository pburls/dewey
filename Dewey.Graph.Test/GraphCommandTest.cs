using Dewey.Graph.Writers;
using Dewey.Manifest;
using Dewey.Messaging;
using Moq;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dewey.Graph.Test
{
    public class GraphCommandTest
    {
        Mock<IGraphGenerator> mockIGraphGenerator;

        Container container;
        ICommandProcessor commandProcessor;

        public GraphCommandTest()
        {
            mockIGraphGenerator = new Mock<IGraphGenerator>();

            container = new Container();
            Messaging.Bootstrapper.RegisterTypes(container);
            File.Bootstrapper.RegisterTypes(container);
            Manifest.Bootstrapper.RegisterTypes(container);
            State.Bootstrapper.RegisterTypes(container);
            Graph.Bootstrapper.RegisterTypes(container);

            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton(mockIGraphGenerator.Object);

            var moduleCataloge = container.GetInstance<ModuleCatalogue>();
            moduleCataloge.Load<State.Module>();
            moduleCataloge.Load<Manifest.Module>();

            commandProcessor = container.GetInstance<ICommandProcessor>();

            commandProcessor.RegisterHandler<GraphCommand, GraphCommandHandler>();
        }

        [Fact]
        public void GraphCommand_Should_GenerateDOTGraphText()
        {
            //Given
            var graphCommand = new GraphCommand(false);
            var writeResult = new WriteGraphResult(null, null);
            mockIGraphGenerator.Setup(x => x.WriteDOTGraph(It.IsAny<string>())).Returns(writeResult);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(graphCommand);

            //Then
            mockIGraphGenerator.Verify(x => x.GenerateDOTGraph(It.IsAny<IEnumerable<Node>>(), It.IsAny<IEnumerable<Edge>>(), It.IsAny<IEnumerable<Layer>>()), Times.Once());
        }

        [Fact]
        public void GraphCommand_Should_DefaultToWriteDOTGraphText()
        {
            //Given
            var graphCommand = new GraphCommand(false);
            var writeResult = new WriteGraphResult(null, null);
            mockIGraphGenerator.Setup(x => x.WriteDOTGraph(It.IsAny<string>())).Returns(writeResult);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(graphCommand);

            //Then
            mockIGraphGenerator.Verify(x => x.WriteDOTGraph(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GraphCommand_Should_WritePNGGraph()
        {
            //Given
            var graphCommand = new GraphCommand(true);
            var writeResult = new WriteGraphResult(null, null);
            mockIGraphGenerator.Setup(x => x.WritePNGGraph(It.IsAny<string>())).Returns(writeResult);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(graphCommand);

            //Then
            mockIGraphGenerator.Verify(x => x.WritePNGGraph(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GraphCommand_Should_DispatchResult()
        {
            //Given
            var graphCommand = new GraphCommand(false);

            var writeResult = new WriteGraphResult(null, null);
            mockIGraphGenerator.Setup(x => x.WriteDOTGraph(It.IsAny<string>())).Returns(writeResult);

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
