using Dewey.File;
using Dewey.Manifest.Events;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Dewey.Manifest.Test
{
    public class ManifestLoadHandlerTest
    {
        readonly Fixture _fixture;
        readonly Mock<IManifestFileReaderService> _mockManifestFileReaderService;
        readonly Mock<IEventAggregator> _mockEventAggregator;

        readonly ManifestLoadHandler _target;

        public ManifestLoadHandlerTest()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new PropertyTypeOmitter(typeof(IManifestFileReader)));

            _mockManifestFileReaderService = new Mock<IManifestFileReaderService>();
            _mockEventAggregator = new Mock<IEventAggregator>();

            _target = new ManifestLoadHandler(_mockEventAggregator.Object, _mockManifestFileReaderService.Object);
        }

        [Fact]
        public void Handle_JsonManifestLoadResult_should_load_ManifestFile_references()
        {
            //Given
            var manifest = _fixture.Build<Manifest.Models.Manifest>()
                .Without(x => x.components)
                .Without(x => x.runtimeResources)
                .Create();
            var manifestFileReader = new MockManifestFileReader() { DirectoryName = "testDirectory" };
            var jsonManifestLoadResult = new JsonManifestLoadResult(manifestFileReader, manifest);

            var childManifestFileReader = new MockManifestFileReader() { Text = "" };
            _mockManifestFileReaderService.Setup(x => x.ReadDeweyManifestFile(manifestFileReader.DirectoryName, It.IsAny<string>())).Returns(childManifestFileReader);

            //When
            _target.Handle(jsonManifestLoadResult);

            //Then
            _mockManifestFileReaderService.Verify(x => x.ReadDeweyManifestFile(manifestFileReader.DirectoryName, It.IsAny<string>()), Times.Exactly(manifest.manifestFiles.Length));
            _mockEventAggregator.Verify(x => x.PublishEvent(It.IsAny<ManifestFileEvent>()), Times.Exactly(manifest.manifestFiles.Length));
        }
    }
}
