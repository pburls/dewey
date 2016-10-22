using Dewey.Build.Events;
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
        Mock<IBuildElementLoader> buildElementLoader;

        Fixture fixture;
        BuildCommandHandler target;

        public BuildCommandHandlerTest()
        {
            commandProcessorMock = new Mock<ICommandProcessor>();
            eventAggregatorMock = new Mock<IEventAggregator>();
            buildActionFactoryMock = new Mock<IBuildActionFactory>();
            buildElementLoader = new Mock<IBuildElementLoader>();

            fixture = new Fixture();
            fixture.Register<File.IManifestFileReader>(() => { return new MockManifestFileReader() { DirectoryName = "test" }; });
            target = new BuildCommandHandler(commandProcessorMock.Object, eventAggregatorMock.Object, buildActionFactoryMock.Object, buildElementLoader.Object);
        }

        //Fixes: Issue #5
        [Fact]
        public void BuildCommandHandler_onlyBuildsDependciesForCommandsComponent()
        {
            //Given
            var buildCommand = new BuildCommand("testComponentName", true);
            var getComponentResult = fixture.Create<GetComponentResult>();
            getComponentResult = getComponentResult.WithCommand(new GetComponent(buildCommand.ComponentName));
            getComponentResult = getComponentResult.WithComponent(getComponentResult.Component.WithComponentMandifest(getComponentResult.Component.ComponentManifest.WithName(getComponentResult.Command.ComponentName)));
            var buildElementResult = fixture.Create<BuildElementResult>();
            buildElementResult = buildElementResult.WithCommand(buildCommand);

            commandProcessorMock.Setup(x => x.Execute(getComponentResult.Command)).Callback<ICommand>((command) =>
            {
                target.Handle(getComponentResult);
            });

            buildElementLoader.Setup(x => x.LoadFromComponentManifest(buildCommand, getComponentResult.Component.ComponentElement)).Callback(() =>
            {
                target.Handle(buildElementResult);
            });

            //When
            target.Execute(buildCommand);

            //Then

        }
    }
}
