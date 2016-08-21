using Dewey.Build.Events;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace Dewey.Build.Test
{
    public class MSBuildTest
    {
        MockFileService fileServiceMock;
        Mock<IEventAggregator> eventAggregatorMock;
        Mock<IMSBuildProcess> msBuildProcessMock;

        MSBuild target;

        Fixture fixture;
        ComponentManifest componentManifest;

        public MSBuildTest()
        {
            fileServiceMock = new MockFileService();
            eventAggregatorMock = new Mock<IEventAggregator>();
            msBuildProcessMock = new Mock<IMSBuildProcess>();
            target = new MSBuild(eventAggregatorMock.Object, fileServiceMock, msBuildProcessMock.Object);

            fixture = new Fixture();
            fixture.Register<File.IManifestFileReader>(() => { return new MockManifestFileReader() { DirectoryName = "test" }; });
            componentManifest = fixture.Create<ComponentManifest>();
        }

        [Fact]
        public void Build_publishes_a_BuildElementMissingAttributeResult_for_a_msbuild_element_missing_an_attribute()
        {
            //Given
            XElement buildElement = XElement.Parse("<build type=\"msbuild\" />");

            var expectedResult = new BuildElementMissingAttributeResult(componentManifest, MSBuild.BUILD_TYPE, buildElement, "target");

            //When
            target.Build(componentManifest, buildElement);

            //Then
            eventAggregatorMock.Verify(x => x.PublishEvent(expectedResult), Times.Once);
        }

        [Fact]
        public void Build_publishes_a_BuildActionTargetNotFoundResult_for_target_path_that_does_not_exist()
        {
            //Given
            XElement buildElement = XElement.Parse("<build type=\"msbuild\" target=\"targetfile\" />");
            fileServiceMock.FileExistsReturns = false;

            string expectedTargetPath = fileServiceMock.CombinePaths("test", "targetfile");
            var expectedResult = new BuildActionTargetNotFoundResult(componentManifest, MSBuild.BUILD_TYPE, expectedTargetPath);

            //When
            target.Build(componentManifest, buildElement);

            //Then
            eventAggregatorMock.Verify(x => x.PublishEvent(expectedResult), Times.Once);
        }

        [Fact]
        public void Build_publishes_a_BuildActionStarted_and_BuildActionCompletedResult_for_a_valid_build_element()
        {
            //Given
            XElement buildElement = XElement.Parse("<build type=\"msbuild\" target=\"targetfile\" />");

            var expectedBuildArgs = new MSBuildArgs("targetfile", new List<string>());
            var expectedResult1 = new BuildActionStarted(componentManifest, MSBuild.BUILD_TYPE, expectedBuildArgs);
            var expectedResult2 = new BuildActionCompletedResult(componentManifest, MSBuild.BUILD_TYPE, expectedBuildArgs);

            //When
            target.Build(componentManifest, buildElement);

            //Then
            eventAggregatorMock.Verify(x => x.PublishEvent(expectedResult1), Times.Once);
            eventAggregatorMock.Verify(x => x.PublishEvent(expectedResult2), Times.Once);
        }
    }
}
