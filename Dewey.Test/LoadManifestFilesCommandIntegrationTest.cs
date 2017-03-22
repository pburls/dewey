using Dewey.File;
using Dewey.Manifest;
using Dewey.Manifest.Events;
using Dewey.Messaging;
using Moq;
using Ploeh.AutoFixture;
using SimpleInjector;
using System.Linq;
using Xunit;

namespace Dewey.Test
{
    public class LoadManifestFilesCommandIntegrationTest
    {
        Mock<IManifestFileReaderService> mockIManifestFileReaderService;

        Container container;
        ICommandProcessor commandProcessor;
        IEventAggregator eventAggregator;

        Fixture fixture;

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

            fixture = new Fixture();
            fixture.Register<File.IManifestFileReader>(() => { return new MockManifestFileReader() { DirectoryName = "test" }; });
        }

        [Fact]
        public void LoadManifestFilesCommand_ForDeweyManifestFileReader_Should_Publish_JsonManifestLoadResult()
        {
            //Given
            var manifest = fixture.Create<Manifest.Models.Manifest>();
            var manifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Dewey, Text = manifest.ToJson() };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var manifestFileReferenceReader = new MockManifestFileReader() { FileExists = false };
            mockIManifestFileReaderService.Setup(x => x.ReadDeweyManifestFile(It.IsAny<string[]>())).Returns(manifestFileReferenceReader);

            var mockJsonManifestLoadResultEventHandler = new Mock<IEventHandler<JsonManifestLoadResult>>();
            eventAggregator.Subscribe(mockJsonManifestLoadResultEventHandler.Object);

            var command = new LoadManifestFiles();

            //When
            commandProcessor.Execute(command);

            //Then
            mockJsonManifestLoadResultEventHandler.Verify(x => x.Handle(It.IsAny<JsonManifestLoadResult>()), Times.Once);
        }

        [Fact]
        public void LoadManifestFilesCommand_ForDeweyManifestFileReader_Should_Publish_JsonManifestLoadResults_for_referenced_manifestFiles()
        {
            //Given
            var manifestFileReferences = fixture.Build<Manifest.Models.ManifestFile>().CreateMany(1).ToArray();

            var rootManifest = fixture.Build<Manifest.Models.Manifest>()
                                      .Without(x => x.components)
                                      .Without(x => x.runtimeResources)
                                      .With(x => x.manifestFiles, manifestFileReferences)
                                      .Create();

            var referencedManifest = fixture.Build<Manifest.Models.Manifest>()
                                            .Without(x => x.manifestFiles)
                                            .Create();

            var rootManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Dewey, Text = rootManifest.ToJson(), DirectoryName = "root" };
            mockIManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(rootManifestFileReader);

            var referencedManifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Dewey, Text = referencedManifest.ToJson() };
            mockIManifestFileReaderService.Setup(x => x.ReadDeweyManifestFile(rootManifestFileReader.DirectoryName, manifestFileReferences[0].location)).Returns(referencedManifestFileReader);

            var mockJsonManifestLoadResultEventHandler = new Mock<IEventHandler<JsonManifestLoadResult>>();
            eventAggregator.Subscribe(mockJsonManifestLoadResultEventHandler.Object);

            var command = new LoadManifestFiles();

            //When
            commandProcessor.Execute(command);

            //Then
            mockJsonManifestLoadResultEventHandler.Verify(x => x.Handle(It.IsAny<JsonManifestLoadResult>()), Times.Exactly(2));
        }
    }
}
