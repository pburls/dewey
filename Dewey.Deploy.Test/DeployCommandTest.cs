using Dewey.File;
using Dewey.Manifest;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using SimpleInjector;
using System.Xml.Linq;
using Xunit;

namespace Dewey.Deploy.Test
{
    public class DeployCommandTest
    {
        Mock<IManifestFileReaderService> mockIManifestFileReaderService;
        Mock<IDeploymentAction> mockDeploymentAction;
        Mock<IDeploymentActionFactory> mockDeploymentActionFactory;

        Container container;
        ICommandProcessor commandProcessor;

        public DeployCommandTest()
        {
            mockIManifestFileReaderService = new Mock<IManifestFileReaderService>();
            mockDeploymentAction = new Mock<IDeploymentAction>();
            mockDeploymentActionFactory = new Mock<IDeploymentActionFactory>();

            container = new Container();
            Messaging.Bootstrapper.RegisterTypes(container);
            File.Bootstrapper.RegisterTypes(container);
            Manifest.Bootstrapper.RegisterTypes(container);
            State.Bootstrapper.RegisterTypes(container);
            Deploy.Bootstrapper.RegisterTypes(container);

            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton(mockIManifestFileReaderService.Object);
            container.RegisterSingleton(mockDeploymentActionFactory.Object);

            var moduleCataloge = container.GetInstance<ModuleCatalogue>();
            moduleCataloge.Load<State.Module>();
            moduleCataloge.Load<Manifest.Module>();
            moduleCataloge.Load<Deploy.Module>();

            commandProcessor = container.GetInstance<ICommandProcessor>();
        }

        [Fact]
        public void DeployCommand_Should_InvokeDeployAction()
        {
            //Given
            var xmlText =
@"<componentManifest name=""testComponent"" type=""web"">
	<deployments>
		<deployment type=""testType"" port=""13319"" siteName=""WebApplication"" appPool=""WebApplication"" content=""src/WebApplication"" />
	</deployments>
</componentManifest>";
            var manifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = xmlText, DirectoryName = "testLocation" };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            mockDeploymentActionFactory.Setup(x => x.CreateDeploymentAction("testType")).Returns(mockDeploymentAction.Object);

            var deployCommand = DeployCommand.Create("testComponent", false);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(deployCommand);

            //Then
            mockDeploymentAction.Verify(x => x.Deploy(It.IsAny<ComponentManifest>(), It.IsAny<XElement>()), Times.Once);
        }

        [Fact]
        public void DeployCommand_Without_Dependenices_Should_Not_InvokeDeployAction_For_Dependencies()
        {
            //Given
            var repositoryDirectory = "testDirectory";
            var repositoryManifestText =
@"<repository name=""TestRepo"">
	<components>
		<component name=""testComponent"" location=""TestLocation"" />
		<component name=""dependencyComponent"" location=""DependencyLocation"" />
	</components>
</repository>";
            var repositoriesManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Repository, XmlText = repositoryManifestText, DirectoryName = repositoryDirectory };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(repositoriesManifestFileReader);

            var testComponentManifestText =
@"<componentManifest name=""testComponent"" type=""web"">
	<deployments>
		<deployment type=""testType"" port=""13319"" siteName=""WebApplication"" appPool=""WebApplication"" content=""src/WebApplication"" />
	</deployments>
	<dependencies>
		<dependency type=""component"" name=""dependencyComponent"" />
	</dependencies>
</componentManifest>";
            var componentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = testComponentManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "TestLocation")).Returns(componentManifestFileReader);

            var dependencyComponentManifestText =
@"<componentManifest name=""dependencyComponent"" type=""web"">
	<deployments>
		<deployment type=""testType"" port=""13319"" siteName=""WebApplication"" appPool=""WebApplication"" content=""src/WebApplication"" />
	</deployments>
</componentManifest>";
            var dependencyComponentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = dependencyComponentManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "DependencyLocation")).Returns(dependencyComponentManifestFileReader);

            mockDeploymentActionFactory.Setup(x => x.CreateDeploymentAction("testType")).Returns(mockDeploymentAction.Object);

            var deployCommand = DeployCommand.Create("testComponent", false);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(deployCommand);

            //Then
            mockDeploymentAction.Verify(x => x.Deploy(It.IsAny<ComponentManifest>(), It.IsAny<XElement>()), Times.Once);
        }

        //Fixes Issue #6
        [Fact]
        public void DeployCommand_With_Dependencies_Should_InvokeBuildAction_For_Dependencies()
        {
            //Given
            var repositoryDirectory = "testDirectory";
            var repositoryManifestText =
@"<repository name=""TestRepo"">
	<components>
		<component name=""testComponent"" location=""TestLocation"" />
		<component name=""dependencyComponent"" location=""DependencyLocation"" />
		<component name=""dependencyComponent2"" location=""DependencyLocation2"" />
	</components>
</repository>";
            var repositoriesManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Repository, XmlText = repositoryManifestText, DirectoryName = repositoryDirectory };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(repositoriesManifestFileReader);

            var testComponentManifestText =
@"<componentManifest name=""testComponent"" type=""web"">
	<deployments>
		<deployment type=""testType"" port=""13319"" siteName=""WebApplication"" appPool=""WebApplication"" content=""src/WebApplication"" />
	</deployments>
	<dependencies>
		<dependency type=""component"" name=""dependencyComponent"" />
	</dependencies>
</componentManifest>";
            var componentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = testComponentManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "TestLocation")).Returns(componentManifestFileReader);

            var dependencyComponentManifestText =
@"<componentManifest name=""dependencyComponent"" type=""web"">
	<deployments>
		<deployment type=""testType"" port=""13319"" siteName=""WebApplication"" appPool=""WebApplication"" content=""src/WebApplication"" />
	</deployments>
	<dependencies>
		<dependency type=""component"" name=""dependencyComponent2"" />
	</dependencies>
</componentManifest>";
            var dependencyComponentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = dependencyComponentManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "DependencyLocation")).Returns(dependencyComponentManifestFileReader);

            var dependencyComponent2ManifestText =
@"<componentManifest name=""dependencyComponent2"" type=""web"">
	<deployments>
		<deployment type=""testType"" port=""13319"" siteName=""WebApplication"" appPool=""WebApplication"" content=""src/WebApplication"" />
	</deployments>
</componentManifest>";
            var dependencyComponent2ManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = dependencyComponent2ManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "DependencyLocation2")).Returns(dependencyComponent2ManifestFileReader);

            var deployCommand = DeployCommand.Create("testComponent", true);

            mockDeploymentActionFactory.Setup(x => x.CreateDeploymentAction("testType")).Returns(mockDeploymentAction.Object);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(deployCommand);

            //Then
            mockDeploymentAction.Verify(x => x.Deploy(It.IsAny<ComponentManifest>(), It.IsAny<XElement>()), Times.Exactly(3));
        }
    }
}
