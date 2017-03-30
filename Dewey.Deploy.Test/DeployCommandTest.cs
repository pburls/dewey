using Dewey.Deploy.Events;
using Dewey.Deploy.Models;
using Dewey.File;
using Dewey.Manifest;
using Dewey.Manifest.Models;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using Ploeh.AutoFixture;
using SimpleInjector;
using System.Linq;
using Xunit;

namespace Dewey.Deploy.Test
{
    public class DeployCommandTest
    {
        Fixture fixture;

        Mock<IManifestFileReaderService> mockIManifestFileReaderService;
        Mock<IDeploymentAction> mockDeploymentAction;
        Mock<IDeploymentActionFactory> mockDeploymentActionFactory;

        Container container;
        ICommandProcessor commandProcessor;
        IEventAggregator eventAggregator;

        public DeployCommandTest()
        {
            fixture = new Fixture();
            fixture.Customizations.Add(new PropertyTypeOmitter(typeof(IManifestFileReader)));

            mockIManifestFileReaderService = new Mock<IManifestFileReaderService>();
            mockDeploymentAction = new Mock<IDeploymentAction>();
            mockDeploymentActionFactory = new Mock<IDeploymentActionFactory>();

            container = new Container();
            Messaging.Bootstrapper.RegisterTypes(container);
            File.Bootstrapper.RegisterTypes(container);
            Manifest.Bootstrapper.RegisterTypes(container);
            Deploy.Bootstrapper.RegisterTypes(container);

            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton(mockIManifestFileReaderService.Object);
            container.RegisterSingleton(mockDeploymentActionFactory.Object);

            var moduleCataloge = container.GetInstance<ModuleCatalogue>();
            moduleCataloge.Load<Manifest.Module>();
            moduleCataloge.Load<Deploy.Module>();

            commandProcessor = container.GetInstance<ICommandProcessor>();
            eventAggregator = container.GetInstance<IEventAggregator>();
        }

        [Fact]
        public void DeployCommand_Should_InvokeDeployAction_For_DeployableComponent()
        {
            //Given
            var components = fixture.Build<DeployableComponent>().CreateMany(3).ToArray();
            var manifest = fixture.Build<Manifest.Models.Manifest>()
                .Without(x => x.manifestFiles)
                .With(x => x.components, components)
                .Create();
            var manifestFileReader = new MockManifestFileReader() { Text = manifest.ToJson(), MandifestFileType = ManifestFileType.Dewey };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var firstComponent = components.First();
            var deployment = firstComponent.deploy;

            mockDeploymentActionFactory.Setup(x => x.CreateDeploymentAction(deployment.type)).Returns(mockDeploymentAction.Object);

            var buildCommand = DeployCommand.Create(firstComponent.name, false);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(buildCommand);

            //Then
            mockDeploymentAction.Verify(x => x.Deploy(firstComponent, deployment), Times.Once);
        }

        [Fact]
        public void DeployCommand_Should_Not_InvokeDeployAction_For_Non_DeployableComponent()
        {
            //Given
            var components = fixture.Build<Component>().CreateMany(3).ToArray();
            var manifest = fixture.Build<Manifest.Models.Manifest>()
                .Without(x => x.manifestFiles)
                .With(x => x.components, components)
                .Create();
            var manifestFileReader = new MockManifestFileReader() { Text = manifest.ToJson(), MandifestFileType = ManifestFileType.Dewey };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var firstComponent = components.First();

            var buildCommand = DeployCommand.Create(firstComponent.name, false);

            var mockNoJsonDeployManifestFoundEventHandler = new Mock<IEventHandler<NoJsonDeployManifestFound>>();

            NoJsonDeployManifestFound noJsonDeployManifestFoundEvent = null;
            mockNoJsonDeployManifestFoundEventHandler.Setup(x => x.Handle(It.IsAny<NoJsonDeployManifestFound>())).Callback<NoJsonDeployManifestFound>(@event => noJsonDeployManifestFoundEvent = @event);

            eventAggregator.Subscribe(mockNoJsonDeployManifestFoundEventHandler.Object);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(buildCommand);

            //Then
            Assert.NotNull(noJsonDeployManifestFoundEvent);
            Assert.Equal(firstComponent.name, noJsonDeployManifestFoundEvent.Component.name);
        }
    }
}
