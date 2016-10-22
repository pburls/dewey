using Dewey.Build.Events;
using Dewey.Manifest.Dependency;
using Dewey.Messaging;
using Dewey.State.Messages;
using Dewey.Test;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dewey.Build.Test
{
    public class BuildCommandHandlerTest
    {
        Mock<ICommandProcessor> commandProcessorMock;
        Mock<IEventAggregator> eventAggregatorMock;
        Mock<IBuildActionFactory> buildActionFactoryMock;
        Mock<IBuildElementLoader> buildElementLoaderMock;
        Mock<IDependencyElementLoader> dependencyElementLoaderMock;

        Fixture fixture;
        BuildCommandHandler target;

        public BuildCommandHandlerTest()
        {
            commandProcessorMock = new Mock<ICommandProcessor>();
            eventAggregatorMock = new Mock<IEventAggregator>();
            buildActionFactoryMock = new Mock<IBuildActionFactory>();
            buildElementLoaderMock = new Mock<IBuildElementLoader>();
            dependencyElementLoaderMock = new Mock<IDependencyElementLoader>();

            fixture = new Fixture();
            fixture.Register<File.IManifestFileReader>(() => { return new MockManifestFileReader() { DirectoryName = "test" }; });
            target = new BuildCommandHandler(commandProcessorMock.Object, eventAggregatorMock.Object, buildActionFactoryMock.Object, buildElementLoaderMock.Object, dependencyElementLoaderMock.Object);
        }

        //Fixes: Issue #5
        [Fact]
        public void BuildCommandHandler_onlyBuildsDependciesForCommandsComponent()
        {
            //Given
            var buildCommand = new BuildCommand("testComponentName", true);
            var getComponentResult = fixture.Create<GetComponentResult>();
            getComponentResult = getComponentResult.WithCommand(new GetComponent(buildCommand.ComponentName))
                                                   .WithComponent(getComponentResult.Component.WithComponentMandifest(getComponentResult.Component.ComponentManifest.WithName(buildCommand.ComponentName)));
            commandProcessorMock.Setup(x => x.Execute(getComponentResult.Command)).Callback<ICommand>((command) =>
            {
                target.Handle(getComponentResult);
            });

            var buildElementResult = fixture.Create<BuildElementResult>();
            buildElementResult = buildElementResult.WithCommand(buildCommand);
            buildElementLoaderMock.Setup(x => x.LoadFromComponentManifest(buildCommand, getComponentResult.Component.ComponentElement)).Callback(() =>
            {
                target.Handle(buildElementResult);
            });

            var dependencyElementResult = fixture.Create<DependencyElementResult>().WithType(DependencyElementResult.COMPONENT_DEPENDENCY_TYPE);
            dependencyElementLoaderMock.Setup(x => x.LoadFromComponentManifest(getComponentResult.Component.ComponentElement)).Callback(() =>
            {
                target.Handle(dependencyElementResult);
            });

            var dependecyBuildCommand = new BuildCommand(dependencyElementResult.Name, true);
            commandProcessorMock.Setup(x => x.Execute(dependecyBuildCommand)).Callback(() =>
            {
                var childDependencyElementResult = fixture.Create<DependencyElementResult>();
                target.Handle(childDependencyElementResult);
            });

            //When
            target.Execute(buildCommand);

            //Then
            commandProcessorMock.Verify(x => x.Execute(dependecyBuildCommand), Times.Once, "A build command was not dispatched for the dependency.");
        }
    }
}
