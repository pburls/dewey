using Dewey.Build.Models;
using Dewey.File;
using Ark3.Command;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using Ploeh.AutoFixture;
using SimpleInjector;
using System.Linq;
using Xunit;
using Dewey.Manifest;

namespace Dewey.Build.Test
{
    public class BuildCommandIntegrationTest
    {
        Fixture fixture;

        Mock<IManifestFileReaderService> mockIManifestFileReaderService;
        Mock<IBuildAction> mockBuildAction;
        Mock<IBuildActionFactory> mockBuildActionFactory;

        Container container;
        ICommandProcessor commandProcessor;

        public BuildCommandIntegrationTest()
        {
            fixture = new Fixture();
            fixture.Customizations.Add(new PropertyTypeOmitter(typeof(IManifestFileReader)));

            mockIManifestFileReaderService = new Mock<IManifestFileReaderService>();
            mockBuildAction = new Mock<IBuildAction>();
            mockBuildActionFactory = new Mock<IBuildActionFactory>();

            container = new Container();
            Messaging.Bootstrapper.RegisterTypes(container);
            File.Bootstrapper.RegisterTypes(container);
            Manifest.Bootstrapper.RegisterTypes(container);
            Build.Bootstrapper.RegisterTypes(container);

            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton(mockIManifestFileReaderService.Object);
            container.RegisterSingleton(mockBuildActionFactory.Object);

            var moduleCataloge = container.GetInstance<ModuleCatalogue>();
            moduleCataloge.Load<Manifest.Module>();
            moduleCataloge.Load<Build.Module>();

            commandProcessor = container.GetInstance<ICommandProcessor>();
        }

        [Fact]
        public void BuildCommand_Should_InvokeBuildAction()
        {
            //Given
            var components = fixture.Build<BuildableComponent>().CreateMany(3).ToArray();
            var manifest = fixture.Build<Manifest.Models.Manifest>()
                .Without(x => x.manifestFiles)
                .With(x => x.components, components)
                .Create();
            var manifestFileReader = new MockManifestFileReader() { Text = manifest.ToJson(), MandifestFileType = ManifestFileType.Dewey };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var firstComponent = components.First();
            var build = firstComponent.build;

            mockBuildActionFactory.Setup(x => x.CreateBuildAction(build.type)).Returns(mockBuildAction.Object);

            var buildCommand = new BuildCommand(firstComponent.name, false);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(buildCommand);

            //Then
            mockBuildAction.Verify(x => x.Build(firstComponent, build), Times.Once);
        }
    }
}
