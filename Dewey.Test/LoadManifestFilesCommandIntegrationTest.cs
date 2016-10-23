using Dewey.File;
using Dewey.Manifest;
using Dewey.Manifest.Component;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using Moq;
using SimpleInjector;
using Xunit;

namespace Dewey.Test
{
    public class LoadManifestFilesCommandIntegrationTest
    {
        Mock<IManifestFileReaderService> mockIManifestFileReaderService;

        Container container;
        ICommandProcessor commandProcessor;
        IEventAggregator eventAggregator;

        public LoadManifestFilesCommandIntegrationTest()
        {
            mockIManifestFileReaderService = new Mock<IManifestFileReaderService>();

            container = new Container();
            Messaging.Bootstrapper.RegisterTypes(container);
            File.Bootstrapper.RegisterTypes(container);
            Manifest.Bootstrapper.RegisterTypes(container);

            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton(mockIManifestFileReaderService.Object);

            var moduleCataloge = container.GetInstance<ModuleCatalogue>();
            moduleCataloge.Load<Manifest.Module>();

            commandProcessor = container.GetInstance<ICommandProcessor>();
            eventAggregator = container.GetInstance<IEventAggregator>();
        }

        [Fact]
        public void LoadManifestFilesCommand_ForComponentManifestFileReader_Should_Publish_ComponentManifestLoadResult()
        {
            //Given
            var xmlText = "<componentManifest name=\"ExampleWebApiComp\" type=\"web\"/>";
            var manifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = xmlText };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var mockComponentManifestLoadResultEventHandler = new Mock<IEventHandler<ComponentManifestLoadResult>>();
            eventAggregator.Subscribe(mockComponentManifestLoadResultEventHandler.Object);

            var command = new LoadManifestFiles();

            //When
            commandProcessor.Execute(command);

            //Then
            mockComponentManifestLoadResultEventHandler.Verify(x => x.Handle(It.IsAny<ComponentManifestLoadResult>()), Times.Once);
        }

        [Fact]
        public void LoadManifestFilesCommand_ForRepositoryManifestFileReader_Should_Publish_RepositoryManifestLoadResult()
        {
            //Given
            var xmlText = "<repository name=\"TestRepo\"/>";
            var manifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Repository, XmlText = xmlText };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var mockRepositoryManifestLoadResultEventHandler = new Mock<IEventHandler<RepositoryManifestLoadResult>>();
            eventAggregator.Subscribe(mockRepositoryManifestLoadResultEventHandler.Object);

            var command = new LoadManifestFiles();

            //When
            commandProcessor.Execute(command);

            //Then
            mockRepositoryManifestLoadResultEventHandler.Verify(x => x.Handle(It.IsAny<RepositoryManifestLoadResult>()), Times.Once);
        }

        [Fact]
        public void LoadManifestFilesCommand_ForRepositoriesManifestFileReader_Should_Publish_RepositoriesManifestLoadResult()
        {
            //Given
            var xmlText = "<repositories/>";
            var manifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Repositories, XmlText = xmlText };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var mockRepositoriesManifestLoadResultEventHandler = new Mock<IEventHandler<RepositoriesManifestLoadResult>>();
            eventAggregator.Subscribe(mockRepositoriesManifestLoadResultEventHandler.Object);

            var command = new LoadManifestFiles();

            //When
            commandProcessor.Execute(command);

            //Then
            mockRepositoriesManifestLoadResultEventHandler.Verify(x => x.Handle(It.IsAny<RepositoriesManifestLoadResult>()), Times.Once);
        }

        [Fact]
        public void LoadManifestFilesCommand_ForRepositoriesManifestFileReader_WithRepositoryElements_Should_Publish_RepositoryManifestLoadResult()
        {
            //Given
            var repositoriesDirectory = "testDirectory";
            var repositoriesXmlText =
@"<repositories>
	<repository name=""TestRepo"" location=""TestLocation"" />
</repositories>";

            var repositoriesManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Repositories, XmlText = repositoriesXmlText, DirectoryName = repositoriesDirectory };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(repositoriesManifestFileReader);

            var repositoryXmlText = "<repository name=\"TestRepo\"/>";
            var repositoryManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Repository, XmlText = repositoryXmlText };
            mockIManifestFileReaderService.Setup(x => x.ReadRepositoryManifestFile(repositoriesDirectory, "TestLocation")).Returns(repositoryManifestFileReader);

            var mockRepositoryManifestLoadResultEventHandler = new Mock<IEventHandler<RepositoryManifestLoadResult>>();
            eventAggregator.Subscribe(mockRepositoryManifestLoadResultEventHandler.Object);

            var command = new LoadManifestFiles();

            //When
            commandProcessor.Execute(command);

            //Then
            mockRepositoryManifestLoadResultEventHandler.Verify(x => x.Handle(It.IsAny<RepositoryManifestLoadResult>()), Times.Once);
        }

        [Fact]
        public void LoadManifestFilesCommand_ForRepositoryManifestFileReader_WithComponentElements_Should_Publish_ComponentManifestLoadResult()
        {
            //Given
            var repositoryDirectory = "testDirectory";
            var repositoryXmlText =
@"<repository name=""TestRepo"">
	<components>
		<component name=""TestComp"" location=""TestLocation"" />
	</components>
</repository>";

            var repositoryManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Repository, XmlText = repositoryXmlText, DirectoryName = repositoryDirectory };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(repositoryManifestFileReader);

            var componentXmlText = "<componentManifest name=\"TestComp\" type=\"web\"/>";
            var componentManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Component, XmlText = componentXmlText };
            mockIManifestFileReaderService.Setup(x => x.ReadComponentManifestFile(repositoryDirectory, "TestLocation")).Returns(componentManifestFileReader);

            var mockComponentManifestLoadResultEventHandler = new Mock<IEventHandler<ComponentManifestLoadResult>>();
            eventAggregator.Subscribe(mockComponentManifestLoadResultEventHandler.Object);

            var command = new LoadManifestFiles();

            //When
            commandProcessor.Execute(command);

            //Then
            mockComponentManifestLoadResultEventHandler.Verify(x => x.Handle(It.IsAny<ComponentManifestLoadResult>()), Times.Once);
        }
    }
}
