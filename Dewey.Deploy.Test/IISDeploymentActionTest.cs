using Dewey.Deploy.Events;
using Dewey.Deploy.Models;
using Dewey.File;
using Dewey.Messaging;
using Dewey.Test;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Dewey.Deploy.Test
{
    public class IISDeploymentActionTest
    {
        MockFileService fileServiceMock;
        Mock<IEventAggregator> eventAggregatorMock;
        Mock<IIISDeployProcess> iisDeployProcessMock;
        Mock<IUserService> userService;

        IISDeploymentAction target;

        Fixture fixture;

        public IISDeploymentActionTest()
        {
            fileServiceMock = new MockFileService();
            eventAggregatorMock = new Mock<IEventAggregator>();
            iisDeployProcessMock = new Mock<IIISDeployProcess>();
            userService = new Mock<IUserService>();
            target = new IISDeploymentAction(eventAggregatorMock.Object, fileServiceMock, iisDeployProcessMock.Object, userService.Object);

            fixture = new Fixture();
            fixture.Register<File.IManifestFileReader>(() => { return new MockManifestFileReader() { DirectoryName = "test" }; });
        }

        [Fact]
        public void Deploy_publishes_a_JsonDeploymentMissingAttributesResult_for_a_msbuild_missing_an_attribute()
        {
            //Given
            var iisDeploy = fixture.Build<IISDeploy>().Without(x => x.appPool).Without(x => x.port).Create();
            var component = fixture.Build<DeployableComponent>().With(x => x.deploy, iisDeploy).Create();
            JsonDeploymentMissingAttributesResult result = null;
            eventAggregatorMock.Setup(x => x.PublishEvent(It.IsAny<JsonDeploymentMissingAttributesResult>())).Callback<JsonDeploymentMissingAttributesResult>(@event => result = @event);

            //When
            target.Deploy(component, iisDeploy);

            //Then
            Assert.NotNull(result);
            Assert.Equal(new string[] { "port", "appPool" }, result.AttributeNames);
        }

        [Fact]
        public void Deploy_publishes_a_JsonDeploymentActionContentNotFoundResult_for_contentPath_that_does_not_exist()
        {
            //Given
            var iisDeploy = fixture.Create<IISDeploy>();
            var component = fixture.Build<DeployableComponent>().With(x => x.deploy, iisDeploy).Create();
            fileServiceMock.DirectoryExistsReturns = false;
            string expectedContentPath = fileServiceMock.CombinePaths(component.File.DirectoryName, iisDeploy.content);

            JsonDeploymentActionContentNotFoundResult result = null;
            eventAggregatorMock.Setup(x => x.PublishEvent(It.IsAny<JsonDeploymentActionContentNotFoundResult>())).Callback<JsonDeploymentActionContentNotFoundResult>(@event => result = @event);

            //When
            target.Deploy(component, iisDeploy);

            //Then
            Assert.NotNull(result);
            Assert.Equal(expectedContentPath, result.ContentPath);
        }

        [Fact]
        public void Deploy_publishes_a_JsonDeploymentActionFailed_for_non_adminstrator_user()
        {
            //Given
            var iisDeploy = fixture.Create<IISDeploy>();
            var component = fixture.Build<DeployableComponent>().With(x => x.deploy, iisDeploy).Create();

            JsonDeploymentActionFailed result = null;
            eventAggregatorMock.Setup(x => x.PublishEvent(It.IsAny<JsonDeploymentActionFailed>())).Callback<JsonDeploymentActionFailed>(@event => result = @event);

            //When
            target.Deploy(component, iisDeploy);

            //Then
            Assert.NotNull(result);
        }

        [Fact]
        public void Deploy_publishes_a_DeploymentActionStarted_and_DeploymentActionCompletedResult_for_a_valid_IISDeploy()
        {
            //Given
            var iisDeploy = fixture.Create<IISDeploy>();
            var component = fixture.Build<DeployableComponent>().With(x => x.deploy, iisDeploy).Create();

            JsonDeploymentActionStarted started = null;
            eventAggregatorMock.Setup(x => x.PublishEvent(It.IsAny<JsonDeploymentActionStarted>())).Callback<JsonDeploymentActionStarted>(@event => started = @event);

            JsonDeploymentActionCompletedResult completed = null;
            eventAggregatorMock.Setup(x => x.PublishEvent(It.IsAny<JsonDeploymentActionCompletedResult>())).Callback<JsonDeploymentActionCompletedResult>(@event => completed = @event);

            userService.Setup(x => x.IsUserAdministrator()).Returns(true);

            //When
            target.Deploy(component, iisDeploy);

            //Then
            Assert.NotNull(started);
            Assert.NotNull(completed);
        }

        [Fact]
        public void Deploy_does_iisDeployProcess_for_a_valid_IISDeploy()
        {
            //Given
            var iisDeploy = fixture.Create<IISDeploy>();
            var component = fixture.Build<DeployableComponent>().With(x => x.deploy, iisDeploy).Create();

            userService.Setup(x => x.IsUserAdministrator()).Returns(true);

            var expectedContentPath = fileServiceMock.CombinePaths(component.File.DirectoryName, iisDeploy.content);

            //When
            target.Deploy(component, iisDeploy);

            //Then
            iisDeployProcessMock.Verify(x => x.Deploy(component, iisDeploy, expectedContentPath), Times.Once);
        }
    }
}
