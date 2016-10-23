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

namespace Dewey.Build.Test
{
    public class BuildCommandIntegrationTest
    {
        Mock<IBuildAction> mockBuildAction;
        Mock<IBuildActionFactory> mockBuildActionFactory;

        Container container;
        ICommandProcessor commandProcessor;

        public BuildCommandIntegrationTest()
        {
            mockBuildAction = new Mock<IBuildAction>();
            mockBuildActionFactory = new Mock<IBuildActionFactory>();

            container = new Container();
            Messaging.Bootstrapper.RegisterTypes(container);
            File.Bootstrapper.RegisterTypes(container);
            Manifest.Bootstrapper.RegisterTypes(container);
            State.Bootstrapper.RegisterTypes(container);
            Build.Bootstrapper.RegisterTypes(container);

            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton(mockBuildActionFactory.Object);

            var moduleCataloge = container.GetInstance<ModuleCatalogue>();
            moduleCataloge.Load<State.Module>();
            moduleCataloge.Load<Manifest.Module>();
            moduleCataloge.Load<Build.Module>();

            commandProcessor = container.GetInstance<ICommandProcessor>();
        }

        [Fact]
        public void Valid_BuildCommand_Should_InvokeBuildAction()
        {
            //Given
            var buildCommand = new BuildCommand("testComponent", false);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(buildCommand);

            //Then
            mockBuildActionFactory.Verify(x => x.CreateBuildAction(It.IsAny<string>()), Times.Once);
        }
    }
}
