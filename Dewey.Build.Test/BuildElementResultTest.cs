using Dewey.Build.Events;
using Dewey.Messaging;
using Moq;
using Ploeh.AutoFixture;
using System.Xml.Linq;
using Xunit;

namespace Dewey.Build.Test
{
    public class BuildElementResultTest
    {
        Mock<IEventAggregator> eventAggregatorMock;

        BuildElementLoader target;

        public BuildElementResultTest()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            target = new BuildElementLoader(eventAggregatorMock.Object);
        }

        [Fact]
        public void LoadBuildElementsFromComponentManifest_publishes_a_NoBuildElementsFoundResult_for_a_componentManifest_without_a_builds_element()
        {
            //Given
            var buildCommand = new Fixture().Create<BuildCommand>();
            XElement componentElement = XElement.Parse("<componentManifest />");

            var expectedResult = new NoBuildElementsFoundResult(buildCommand, componentElement);

            //When
            target.LoadFromComponentManifest(buildCommand, componentElement);

            //Then
            eventAggregatorMock.Verify(x => x.PublishEvent(expectedResult), Times.Once);
        }

        [Fact]
        public void LoadBuildElementsFromComponentManifest_publishes_a_NoBuildElementsFoundResult_for_a_componentManifest_without_any_build_elements()
        {
            //Given
            var buildCommand = new Fixture().Create<BuildCommand>();
            XElement componentElement = XElement.Parse("<componentManifest><builds /></componentManifest>");

            var expectedResult = new NoBuildElementsFoundResult(buildCommand, componentElement);

            //When
            target.LoadFromComponentManifest(buildCommand, componentElement);

            //Then
            eventAggregatorMock.Verify(x => x.PublishEvent(expectedResult), Times.Once);
        }

        [Fact]
        public void LoadBuildElementsFromComponentManifest_publishes_a_BuildElementMissingTypeAttributeResult_for_a_build_element_without_a_type()
        {
            //Given
            var buildCommand = new Fixture().Create<BuildCommand>();
            XElement componentElement = XElement.Parse("<componentManifest/>");
            XElement buildsElement = XElement.Parse("<builds/>");
            componentElement.Add(buildsElement);
            XElement buildElement1 = XElement.Parse("<build/>");
            XElement buildElement2 = XElement.Parse("<build/>");
            buildsElement.Add(buildElement1);
            buildsElement.Add(buildElement2);

            var expectedResult1 = new BuildElementMissingTypeAttributeResult(buildCommand, buildElement1);
            var expectedResult2 = new BuildElementMissingTypeAttributeResult(buildCommand, buildElement2);

            //When
            target.LoadFromComponentManifest(buildCommand, componentElement);

            //Then
            eventAggregatorMock.Verify(x => x.PublishEvent(expectedResult1), Times.Once);
            eventAggregatorMock.Verify(x => x.PublishEvent(expectedResult2), Times.Once);
        }

        [Fact]
        public void LoadBuildElementsFromComponentManifest_publishes_a_BuildElementResult_for_a_build_element_with_a_type()
        {
            //Given
            var buildCommand = new Fixture().Create<BuildCommand>();
            XElement componentElement = XElement.Parse("<componentManifest/>");
            XElement buildsElement = XElement.Parse("<builds/>");
            componentElement.Add(buildsElement);
            XElement buildElement1 = XElement.Parse("<build type=\"a\" />");
            XElement buildElement2 = XElement.Parse("<build type=\"b\" />");
            buildsElement.Add(buildElement1);
            buildsElement.Add(buildElement2);

            var expectedResult1 = new BuildElementResult(buildCommand, buildElement1, "a");
            var expectedResult2 = new BuildElementResult(buildCommand, buildElement2, "b");

            //When
            target.LoadFromComponentManifest(buildCommand, componentElement);

            //Then
            eventAggregatorMock.Verify(x => x.PublishEvent(expectedResult1), Times.Once);
            eventAggregatorMock.Verify(x => x.PublishEvent(expectedResult2), Times.Once);
        }
    }
}
