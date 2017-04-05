using Dewey.Build.Models;
using Dewey.Manifest.Messages;
using Dewey.Manifest.Models;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using Ploeh.AutoFixture;
using System.Linq;
using Xunit;

namespace Dewey.Build.Test
{
    public class BuildCommandHandlerTest
    {
        Mock<ICommandProcessor> commandProcessorMock;
        Mock<IEventAggregator> eventAggregatorMock;
        Mock<IBuildActionFactory> buildActionFactoryMock;
        Mock<IBuildAction> buildActionMock;
        Mock<IBuildCommandCache> buildCommandCacheMock;

        Fixture fixture;
        BuildCommandHandler target;

        public BuildCommandHandlerTest()
        {
            commandProcessorMock = new Mock<ICommandProcessor>();
            eventAggregatorMock = new Mock<IEventAggregator>();
            buildActionFactoryMock = new Mock<IBuildActionFactory>();
            buildActionMock = new Mock<IBuildAction>();
            buildCommandCacheMock = new Mock<IBuildCommandCache>();

            buildActionFactoryMock.Setup(x => x.CreateBuildAction(It.IsAny<string>())).Returns(buildActionMock.Object);

            fixture = new Fixture();
            fixture.Register<File.IManifestFileReader>(() => { return new MockManifestFileReader() { DirectoryName = "test" }; });
            target = new BuildCommandHandler(commandProcessorMock.Object, eventAggregatorMock.Object, buildActionFactoryMock.Object, buildCommandCacheMock.Object);
        }

        [Fact]
        public void BuildCommandHandler_dispatches_BuildCommands_for_Dependencies_of_type_Component_only()
        {
            //Given
            var buildCommand = new BuildCommand("testComponentName", true);
            var componentDependencies = fixture.Build<Dependency>().With(x => x.type, "component").CreateMany(2);
            var otherDependencies = fixture.Build<Dependency>().CreateMany(3);
            var component = fixture.Build<BuildableComponent>()
                                   .With(x => x.name, buildCommand.ComponentName)
                                   .With(x => x.dependencies, componentDependencies.Union(otherDependencies).ToArray())
                                   .Create();

            var getComponent = new GetComponent(buildCommand.ComponentName);
            var getComponentResult = new GetComponentResult(getComponent, component);
            commandProcessorMock.Setup(x => x.Execute(getComponentResult.Command)).Callback<ICommand>((command) =>
            {
                target.Handle(getComponentResult);
            });

            //When
            target.Execute(buildCommand);

            //Then
            commandProcessorMock.Verify(x => x.Execute(It.IsAny<BuildCommand>()), Times.Exactly(componentDependencies.Count()), "A build command was not dispatched for each dependency.");
        }

        [Fact]
        public void BuildCommandHandler_does_not_dispatch_BuildCommands_for_Dependencies_for_component_only_build_command()
        {
            //Given
            var buildCommand = new BuildCommand("testComponentName", false);
            var componentDependencies = fixture.Build<Dependency>().With(x => x.type, "component").CreateMany(3).ToArray();
            var component = fixture.Build<BuildableComponent>()
                                   .With(x => x.name, buildCommand.ComponentName)
                                   .With(x => x.dependencies, componentDependencies)
                                   .Create();

            var getComponent = new GetComponent(buildCommand.ComponentName);
            var getComponentResult = new GetComponentResult(getComponent, component);
            commandProcessorMock.Setup(x => x.Execute(getComponentResult.Command)).Callback<ICommand>((command) =>
            {
                target.Handle(getComponentResult);
            });

            //When
            target.Execute(buildCommand);

            //Then
            commandProcessorMock.Verify(x => x.Execute(It.IsAny<BuildCommand>()), Times.Never, "A build command was incorrectly dispatched for a dependency.");
        }

        [Fact]
        public void BuildCommandHandler_does_not_InvokeBuildAction_for_Components_Already_Built()
        {
            //Given
            var buildCommand = new BuildCommand("testComponentName", false);
            var component = fixture.Build<BuildableComponent>()
                                   .With(x => x.name, buildCommand.ComponentName)
                                   .Create();

            var getComponent = new GetComponent(buildCommand.ComponentName);
            var getComponentResult = new GetComponentResult(getComponent, component);
            commandProcessorMock.Setup(x => x.Execute(getComponentResult.Command)).Callback<ICommand>((command) =>
            {
                target.Handle(getComponentResult);
            });

            buildCommandCacheMock.Setup(x => x.IsComponentAlreadyBuilt(buildCommand.ComponentName)).Returns(true);


            //When
            target.Execute(buildCommand);

            //Then
            buildActionMock.Verify(x => x.Build(It.IsAny<Component>(), It.IsAny<Models.Build>()), Times.Never);
        }
    }
}
