using Dewey.File;
using Dewey.Manifest.Messages;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using SimpleInjector;
using Xunit;

namespace Dewey.Manifest.Test
{
    public class DeweyManifestIntegrationTest
    {
        readonly Fixture _fixture;
        readonly Container _container;
        readonly IEventAggregator _eventAggregator;
        readonly ICommandProcessor _commandProcessor;
        readonly Module _module;
        readonly Mock<IManifestFileReaderService> _mockManifestFileReaderService;

        public DeweyManifestIntegrationTest()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new PropertyTypeOmitter(typeof(IManifestFileReader)));


            _container = new Container();
            _mockManifestFileReaderService = new Mock<IManifestFileReaderService>();

            Messaging.Bootstrapper.RegisterTypes(_container);
            Manifest.Bootstrapper.RegisterTypes(_container);
            _container.RegisterSingleton(_mockManifestFileReaderService.Object);

            _commandProcessor = _container.GetInstance<ICommandProcessor>();
            _eventAggregator = _container.GetInstance<IEventAggregator>();
            _module = _container.GetInstance<Module>();
        }

        [Fact]
        public void ManifestStore_returns_LoadManifestFiles_RuntimeResources_with_GetRuntimeResources()
        {
            //Given
            var manifest = _fixture.Build<Manifest.Models.Manifest>().Without(x => x.manifestFiles).Create();
            var manifestFileReader = new MockManifestFileReader() { Text = manifest.ToJson(), MandifestFileType = ManifestFileType.Dewey };
            _mockManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var mockGetRuntimeResourcesResultEventHandler = new Mock<IEventHandler<GetRuntimeResourcesResult>>();
            _eventAggregator.Subscribe(mockGetRuntimeResourcesResultEventHandler.Object);

            //When
            _commandProcessor.Execute(new Manifest.LoadManifestFiles());
            _commandProcessor.Execute(new Manifest.Messages.GetRuntimeResources());

            //Then
            mockGetRuntimeResourcesResultEventHandler.Verify(x => x.Handle(It.IsAny<GetRuntimeResourcesResult>()), Times.Once);
        }

        [Fact]
        public void ManifestStore_returns_LoadManifestFiles_Components_with_GetComponents()
        {
            //Given
            var manifest = _fixture.Build<Manifest.Models.Manifest>().Without(x => x.manifestFiles).Create();
            var manifestFileReader = new MockManifestFileReader() { Text = manifest.ToJson(), MandifestFileType = ManifestFileType.Dewey };
            _mockManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var mockGetComponentsResultEventHandler = new Mock<IEventHandler<GetComponentsResult>>();
            _eventAggregator.Subscribe(mockGetComponentsResultEventHandler.Object);
            
            //When
            _commandProcessor.Execute(new Manifest.LoadManifestFiles());
            _commandProcessor.Execute(new Manifest.Messages.GetComponents());

            //Then
            mockGetComponentsResultEventHandler.Verify(x => x.Handle(It.IsAny<GetComponentsResult>()), Times.Once);
        }
    }
}
