using Dewey.Build.Events;
using Dewey.Build.Models;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Dewey.Build.Test
{
    public class MSBuildActionTest
    {
        MockFileService fileServiceMock;
        Mock<IEventAggregator> eventAggregatorMock;
        Mock<IMSBuildProcess> msBuildProcessMock;

        MSBuildAction target;

        Fixture fixture;

        public MSBuildActionTest()
        {
            fileServiceMock = new MockFileService();
            eventAggregatorMock = new Mock<IEventAggregator>();
            msBuildProcessMock = new Mock<IMSBuildProcess>();
            target = new MSBuildAction(eventAggregatorMock.Object, fileServiceMock, msBuildProcessMock.Object);

            fixture = new Fixture();
            fixture.Register<File.IManifestFileReader>(() => { return new MockManifestFileReader() { DirectoryName = "test" }; });
        }

        [Fact]
        public void Build_publishes_a_JsonBuildMissingAttributesResult_for_a_msbuild_missing_an_attribute()
        {
            //Given
            var msbuild = fixture.Build<MSBuild>().Without(x => x.msbuildVersion).Without(x => x.target).Create();
            var component = fixture.Build<BuildableComponent>().With(x => x.build, msbuild).Create();
            JsonBuildMissingAttributesResult result  = null;
            eventAggregatorMock.Setup(x => x.PublishEvent(It.IsAny<JsonBuildMissingAttributesResult>())).Callback<JsonBuildMissingAttributesResult>(@event => result = @event);

            //When
            target.Build(component, msbuild);

            //Then
            Assert.NotNull(result);
            Assert.Equal(new string[] { "target", "msbuildVersion" }, result.AttributeNames);
        }

        [Fact]
        public void Build_publishes_a_JsonBuildActionTargetNotFoundResult_for_target_path_that_does_not_exist()
        {
            //Given
            var msbuild = fixture.Create<MSBuild>();
            var component = fixture.Build<BuildableComponent>().With(x => x.build, msbuild).Create();
            fileServiceMock.FileExistsReturns = false;
            string expectedTargetPath = fileServiceMock.CombinePaths(component.File.DirectoryName, msbuild.target);

            JsonBuildActionTargetNotFoundResult result = null;
            eventAggregatorMock.Setup(x => x.PublishEvent(It.IsAny<JsonBuildActionTargetNotFoundResult>())).Callback<JsonBuildActionTargetNotFoundResult>(@event => result = @event);

            //When
            target.Build(component, msbuild);

            //Then
            Assert.NotNull(result);
            Assert.Equal(expectedTargetPath, result.Target);
        }

        [Fact]
        public void Build_publishes_a_JsonMSBuildExecutableNotFoundResult_for_msbuild_version_that_does_not_exist()
        {
            //Given
            var msbuild = fixture.Create<MSBuild>();
            var component = fixture.Build<BuildableComponent>().With(x => x.build, msbuild).Create();

            JsonMSBuildExecutableNotFoundResult result = null;
            eventAggregatorMock.Setup(x => x.PublishEvent(It.IsAny<JsonMSBuildExecutableNotFoundResult>())).Callback<JsonMSBuildExecutableNotFoundResult>(@event => result = @event);

            //When
            target.Build(component, msbuild);

            //Then
            Assert.NotNull(result);
            Assert.Equal(msbuild.msbuildVersion, result.MSBuildVersion);
        }

        [Fact]
        public void Build_publishes_a_BuildActionStarted_and_BuildActionCompletedResult_for_a_valid_msbuild()
        {
            //Given
            var msbuild = fixture.Create<MSBuild>();
            var component = fixture.Build<BuildableComponent>().With(x => x.build, msbuild).Create();

            JsonBuildActionStarted buildActionStarted = null;
            eventAggregatorMock.Setup(x => x.PublishEvent(It.IsAny<JsonBuildActionStarted>())).Callback<JsonBuildActionStarted>(@event => buildActionStarted = @event);

            JsonBuildActionCompletedResult buildActionCompletedResult = null;
            eventAggregatorMock.Setup(x => x.PublishEvent(It.IsAny<JsonBuildActionCompletedResult>())).Callback<JsonBuildActionCompletedResult>(@event => buildActionCompletedResult = @event);
            
            msBuildProcessMock.Setup(x => x.GetMSBuildExecutablePathForVersion(It.IsAny<string>())).Returns("testpath");

            //When
            target.Build(component, msbuild);

            //Then
            Assert.NotNull(buildActionStarted);
            Assert.NotNull(buildActionCompletedResult);
        }
    }
}
