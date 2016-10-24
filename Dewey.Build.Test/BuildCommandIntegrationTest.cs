using Dewey.File;
using Dewey.Manifest;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using SimpleInjector;
using System.Xml.Linq;
using Xunit;

namespace Dewey.Build.Test
{
    public class BuildCommandIntegrationTest
    {
        Mock<IManifestFileReaderService> mockIManifestFileReaderService;
        Mock<IBuildAction> mockBuildAction;
        Mock<IBuildActionFactory> mockBuildActionFactory;

        Container container;
        ICommandProcessor commandProcessor;

        public BuildCommandIntegrationTest()
        {
            mockIManifestFileReaderService = new Mock<IManifestFileReaderService>();
            mockBuildAction = new Mock<IBuildAction>();
            mockBuildActionFactory = new Mock<IBuildActionFactory>();

            container = new Container();
            Messaging.Bootstrapper.RegisterTypes(container);
            File.Bootstrapper.RegisterTypes(container);
            Manifest.Bootstrapper.RegisterTypes(container);
            State.Bootstrapper.RegisterTypes(container);
            Build.Bootstrapper.RegisterTypes(container);

            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton(mockIManifestFileReaderService.Object);
            container.RegisterSingleton(mockBuildActionFactory.Object);

            var moduleCataloge = container.GetInstance<ModuleCatalogue>();
            moduleCataloge.Load<State.Module>();
            moduleCataloge.Load<Manifest.Module>();
            moduleCataloge.Load<Build.Module>();

            commandProcessor = container.GetInstance<ICommandProcessor>();
        }

        [Fact]
        public void BuildCommand_Should_InvokeBuildAction()
        {
            //Given
            var xmlText =
@"<componentManifest name=""testComponent"" type=""web"">
	<builds>
		<build type=""testType"" target=""testTarget"" />
	</builds>
</componentManifest>";
            var manifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = xmlText };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            mockBuildActionFactory.Setup(x => x.CreateBuildAction("testType")).Returns(mockBuildAction.Object);

            var buildCommand = new BuildCommand("testComponent", false);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(buildCommand);

            //Then
            mockBuildAction.Verify(x => x.Build(It.IsAny<ComponentManifest>(), It.IsAny<XElement>()), Times.Once);
        }

        [Fact]
        public void BuildCommand_Without_Dependencies_Should_Not_InvokeBuildAction_For_Dependencies()
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
	<builds>
		<build type=""testType"" target=""testTarget"" />
	</builds>
	<dependencies>
		<dependency type=""component"" name=""dependencyComponent"" />
	</dependencies>
</componentManifest>";
            var componentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = testComponentManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "TestLocation")).Returns(componentManifestFileReader);

            var dependencyComponentManifestText =
@"<componentManifest name=""dependencyComponent"" type=""web"">
	<builds>
		<build type=""testType"" target=""testTarget"" />
	</builds>
</componentManifest>";
            var dependencyComponentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = dependencyComponentManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "DependencyLocation")).Returns(dependencyComponentManifestFileReader);

            var buildCommand = new BuildCommand("testComponent", false);

            mockBuildActionFactory.Setup(x => x.CreateBuildAction("testType")).Returns(mockBuildAction.Object);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(buildCommand);

            //Then
            mockBuildAction.Verify(x => x.Build(It.IsAny<ComponentManifest>(), It.IsAny<XElement>()), Times.Once);
        }

        //Fixes: Issue #5
        [Fact]
        public void BuildCommand_With_Dependencies_Should_InvokeBuildAction_For_Dependencies()
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
	<builds>
		<build type=""testType"" target=""testTarget"" />
	</builds>
	<dependencies>
		<dependency type=""component"" name=""dependencyComponent"" />
	</dependencies>
</componentManifest>";
            var componentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = testComponentManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "TestLocation")).Returns(componentManifestFileReader);

            var dependencyComponentManifestText =
@"<componentManifest name=""dependencyComponent"" type=""web"">
	<builds>
		<build type=""testType"" target=""testTarget"" />
	</builds>
	<dependencies>
		<dependency type=""component"" name=""dependencyComponent2"" />
	</dependencies>
</componentManifest>";
            var dependencyComponentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = dependencyComponentManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "DependencyLocation")).Returns(dependencyComponentManifestFileReader);

            var dependencyComponent2ManifestText =
@"<componentManifest name=""dependencyComponent2"" type=""web"">
	<builds>
		<build type=""testType"" target=""testTarget"" />
	</builds>
</componentManifest>";
            var dependencyComponent2ManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = dependencyComponent2ManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "DependencyLocation2")).Returns(dependencyComponent2ManifestFileReader);

            var buildCommand = new BuildCommand("testComponent", true);

            mockBuildActionFactory.Setup(x => x.CreateBuildAction("testType")).Returns(mockBuildAction.Object);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(buildCommand);

            //Then
            mockBuildAction.Verify(x => x.Build(It.IsAny<ComponentManifest>(), It.IsAny<XElement>()), Times.Exactly(3));
        }

        [Fact]
        public void BuildCommand_With_Dependencies_Should_Not_InvokeBuildAction_For_Dependencies_That_Do_Not_Exist()
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
	<builds>
		<build type=""testType"" target=""testTarget"" />
	</builds>
	<dependencies>
		<dependency type=""component"" name=""dependencyComponent"" />
		<dependency type=""component"" name=""dependencyComponent2"" />
	</dependencies>
</componentManifest>";
            var componentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = testComponentManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "TestLocation")).Returns(componentManifestFileReader);

            var dependencyComponentManifestText =
@"<componentManifest name=""dependencyComponent"" type=""web"">
	<builds>
		<build type=""testType"" target=""testTarget"" />
	</builds>
	<dependencies>
		<dependency type=""component"" name=""missingDependencyComponent"" />
	</dependencies>
</componentManifest>";
            var dependencyComponentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = dependencyComponentManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "DependencyLocation")).Returns(dependencyComponentManifestFileReader);

            var dependencyComponent2ManifestText =
@"<componentManifest name=""dependencyComponent2"" type=""web"">
	<builds>
		<build type=""testType"" target=""testTarget"" />
	</builds>
	<dependencies>
		<dependency type=""component"" name=""missingDependencyComponent2"" />
	</dependencies>
</componentManifest>";
            var dependencyComponent2ManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = dependencyComponent2ManifestText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "DependencyLocation2")).Returns(dependencyComponent2ManifestFileReader);

            var buildCommand = new BuildCommand("testComponent", true);

            mockBuildActionFactory.Setup(x => x.CreateBuildAction("testType")).Returns(mockBuildAction.Object);

            //When
            commandProcessor.Execute(new LoadManifestFiles());
            commandProcessor.Execute(buildCommand);

            //Then
            mockBuildAction.Verify(x => x.Build(It.IsAny<ComponentManifest>(), It.IsAny<XElement>()), Times.Exactly(3));
        }
    }
}
