using Dewey.Deploy.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Dewey.Deploy.Test
{
    public class IISDeployTest
    {
        [Fact]
        public void IISDeploy_valid()
        {
            //Given
            var json = "{ \"type\": \"iis\", \"port\": \"53971\", \"siteName\": \"ExampleWebApiApplication\",  \"appPool\": \"ExampleWebApiApplication\", \"content\": \"src/ExampleWebApiApplication/\" }";
            var jObject = JObject.Parse(json);

            //When
            var target = new IISDeploy(new Models.Deploy(jObject));

            //Then
            Assert.Equal("iis", target.type);
            Assert.Equal(53971, target.port);
            Assert.Equal("ExampleWebApiApplication", target.siteName);
            Assert.Equal("ExampleWebApiApplication", target.appPool);
            Assert.Equal("src/ExampleWebApiApplication/", target.content);
        }

        [Fact]
        public void IISDeploy_invalid_port()
        {
            //Given
            var json = "{ \"type\": \"iis\", \"port\": \"abc\", \"siteName\": \"ExampleWebApiApplication\",  \"appPool\": \"ExampleWebApiApplication\", \"content\": \"src/ExampleWebApiApplication/\" }";
            var jObject = JObject.Parse(json);
            var target = new IISDeploy(new Models.Deploy(jObject));

            //When
            var invalidAtts = target.GetInvalidAttributes();

            //Then
            Assert.Contains("port", invalidAtts);
        }

        [Fact]
        public void IISDeploy_missing_values()
        {
            //Given
            var json = "{ \"type\": \"iis\" }";
            var jObject = JObject.Parse(json);
            var target = new IISDeploy(new Models.Deploy(jObject));

            //When
            var atts = target.GetMissingAttributes();

            //Then
            Assert.Contains("port", atts);
            Assert.Contains("siteName", atts);
            Assert.Contains("appPool", atts);
        }
    }
}
