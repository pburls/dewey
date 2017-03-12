using Dewey.File;
using Dewey.Manifest.Messages;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dewey.Manifest.Test
{
    public class DeweyManifestTest
    {
        readonly Container _container;
        readonly IEventAggregator _eventAggregator;
        readonly ICommandProcessor _commandProcessor;
        readonly Module _module;
        readonly Mock<IManifestFileReaderService> _mockManifestFileReaderService;

        public DeweyManifestTest()
        {
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
        public void ManifestStore_returns_LoadManifestFiles_Component_with_GetComponent()
        {
            //Given
            var manifest = new Fixture().Create<Models.Manifest>();
            var manifestFileReader = new MockManifestFileReader() { Text = JsonConvert.SerializeObject(manifest), MandifestFileType = ManifestFileType.Dewey };
            _mockManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var mockGetComponentResultEventHandler = new Mock<IEventHandler<GetComponentResult>>();
            _eventAggregator.Subscribe(mockGetComponentResultEventHandler.Object);
            GetComponentResult getComponentResult = null;
            mockGetComponentResultEventHandler.Setup(x => x.Handle(It.IsAny<GetComponentResult>())).Callback<GetComponentResult>(result => getComponentResult = result);

            var expectedComponent = manifest.components.First();

            //When
            _commandProcessor.Execute(new Manifest.LoadManifestFiles());
            _commandProcessor.Execute(new Manifest.Messages.GetComponent(expectedComponent.name));

            //Then
            Assert.NotNull(getComponentResult);
            Assert.Equal(expectedComponent, getComponentResult.Component);
        }

        [Fact]
        public void ManifestStore_returns_LoadManifestFiles_RuntimeResources_with_GetRuntimeResources()
        {
            //Given
            var manifest = new Fixture().Create<Models.Manifest>();
            var manifestFileReader = new MockManifestFileReader() { Text = JsonConvert.SerializeObject(manifest), MandifestFileType = ManifestFileType.Dewey };
            _mockManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var mockGetRuntimeResourcesResultEventHandler = new Mock<IEventHandler<GetRuntimeResourcesResult>>();
            _eventAggregator.Subscribe(mockGetRuntimeResourcesResultEventHandler.Object);
            GetRuntimeResourcesResult getRuntimeResourcesResult = null;
            mockGetRuntimeResourcesResultEventHandler.Setup(x => x.Handle(It.IsAny<GetRuntimeResourcesResult>())).Callback<GetRuntimeResourcesResult>(result => getRuntimeResourcesResult = result);

            var expectedComponent = manifest.components.First();

            //When
            _commandProcessor.Execute(new Manifest.LoadManifestFiles());
            _commandProcessor.Execute(new Manifest.Messages.GetRuntimeResources());

            //Then
            Assert.NotNull(getRuntimeResourcesResult);
            Assert.Equal(manifest.runtimeResources, getRuntimeResourcesResult.RuntimeResources.Values);
        }

        [Fact]
        public void ManifestStore_returns_LoadManifestFiles_Components_with_GetComponents()
        {
            //Given
            var manifest = new Fixture().Create<Models.Manifest>();
            var manifestFileReader = new MockManifestFileReader() { Text = JsonConvert.SerializeObject(manifest), MandifestFileType = ManifestFileType.Dewey };
            _mockManifestFileReaderService.Setup(x => x.FindManifestFileInCurrentDirectory()).Returns(manifestFileReader);

            var mockGetComponentsResultEventHandler = new Mock<IEventHandler<GetComponentsResult>>();
            _eventAggregator.Subscribe(mockGetComponentsResultEventHandler.Object);
            GetComponentsResult getComponentsResult = null;
            mockGetComponentsResultEventHandler.Setup(x => x.Handle(It.IsAny<GetComponentsResult>())).Callback<GetComponentsResult>(result => getComponentsResult = result);
            
            //When
            _commandProcessor.Execute(new Manifest.LoadManifestFiles());
            _commandProcessor.Execute(new Manifest.Messages.GetComponents());

            //Then
            Assert.NotNull(getComponentsResult);
            Assert.Equal(manifest.components, getComponentsResult.Components);
        }
    }
}
