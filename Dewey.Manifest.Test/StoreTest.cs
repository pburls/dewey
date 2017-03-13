using Dewey.File;
using Dewey.Manifest.Events;
using Dewey.Manifest.Messages;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using Ploeh.AutoFixture;
using System.Linq;
using Xunit;

namespace Dewey.Manifest.Test
{
    public class StoreTest
    {
        readonly Fixture _fixture;
        readonly Mock<ICommandProcessor> _mockCommandProcessor;
        readonly Mock<IEventAggregator> _mockEventAggregator;

        readonly Store _target;

        public StoreTest()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new PropertyTypeOmitter(typeof(IManifestFileReader)));

            _mockCommandProcessor = new Mock<ICommandProcessor>();
            _mockEventAggregator = new Mock<IEventAggregator>();

            _target = new Store(_mockEventAggregator.Object, _mockCommandProcessor.Object);
        }

        [Fact]
        public void Handle_JsonManifestLoadResult_should_store_components_by_name()
        {
            //Given
            var manifest = _fixture.Build<Models.Manifest>().Create();
            var jsonManifestLoadResult = new JsonManifestLoadResult(null, manifest);

            var expectedComponent = manifest.components.First();

            GetComponentResult actualGetComponentResult = null;
            _mockEventAggregator.Setup(x => x.PublishEvent(It.IsAny<GetComponentResult>())).Callback<GetComponentResult>(result => actualGetComponentResult = result);

            //When
            _target.Handle(jsonManifestLoadResult);
            _target.Execute(new GetComponent(expectedComponent.name));

            //Then
            Assert.NotNull(actualGetComponentResult);
            Assert.Equal(expectedComponent, actualGetComponentResult.Component);
        }

        [Fact]
        public void Handle_JsonManifestLoadResult_should_store_all_components()
        {
            //Given
            var manifest = _fixture.Build<Models.Manifest>().Create();
            var jsonManifestLoadResult = new JsonManifestLoadResult(null, manifest);

            GetComponentsResult actualGetComponentsResult = null;
            _mockEventAggregator.Setup(x => x.PublishEvent(It.IsAny<GetComponentsResult>())).Callback<GetComponentsResult>(result => actualGetComponentsResult = result);

            //When
            _target.Handle(jsonManifestLoadResult);
            _target.Execute(new GetComponents());

            //Then
            Assert.NotNull(actualGetComponentsResult);
            Assert.Equal(manifest.components, actualGetComponentsResult.Components);
        }

        [Fact]
        public void Handle_JsonManifestLoadResult_should_store_all_runtime_resources()
        {
            //Given
            var manifest = _fixture.Build<Models.Manifest>().Create();
            var jsonManifestLoadResult = new JsonManifestLoadResult(null, manifest);

            GetRuntimeResourcesResult actualGetRuntimeResourcesResult = null;
            _mockEventAggregator.Setup(x => x.PublishEvent(It.IsAny<GetRuntimeResourcesResult>())).Callback<GetRuntimeResourcesResult>(result => actualGetRuntimeResourcesResult = result);

            //When
            _target.Handle(jsonManifestLoadResult);
            _target.Execute(new GetRuntimeResources());

            //Then
            Assert.NotNull(actualGetRuntimeResourcesResult);
            Assert.Equal(manifest.runtimeResources, actualGetRuntimeResourcesResult.RuntimeResources.Values);
        }
    }
}
