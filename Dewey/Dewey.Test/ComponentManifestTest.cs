using Dewey.Manfiest;
using Dewey.Manifest.Component;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace Dewey.Test
{
    public class ComponentManifestTest
    {
        static IEnumerable<object[]> GetUnsuccessfulMockManifestFileReaders()
        {
            yield return new object[] { new MockManifestFileReader() { ScenarioName = "with missing directory", FileExists = false, DirectoryExists = false  } };
            yield return new object[] { new MockManifestFileReader() { ScenarioName = "with missing file", FileExists = false, DirectoryExists = true  } };

            string xmlText = "<componentManifest/>";
            yield return new object[] { new MockManifestFileReader() { ScenarioName = "with xml missing all attributes", XmlText = xmlText } };

            xmlText = "<componentManifest type=\"web\"/>";
            yield return new object[] { new MockManifestFileReader() { ScenarioName = "with xml missing name attribute", XmlText = xmlText } };

            xmlText = "<componentManifest name=\"ExampleWebApiComp\"/>";
            yield return new object[] { new MockManifestFileReader() { ScenarioName = "with xml missing type attribute", XmlText = xmlText } };
        }

        [Theory]
        [MemberData(nameof(GetUnsuccessfulMockManifestFileReaders))]
        public void LoadComponentItem_returns_UnsuccessfulResult_for(MockManifestFileReader mockManifestFileReader)
        {
            //Given
            var componentItem = new Fixture().Create<ComponentItem>();

            //When
            var result = ComponentManifest.LoadComponentItem(componentItem, "root", mockManifestFileReader.CreateService());

            //Then
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void LoadComponentItem_returns_SuccessfulResult_for_complete_componentManifest_element()
        {
            //Given
            var mockManifestFileReader = new MockManifestFileReader() { XmlText = "<componentManifest name=\"ExampleWebApiComp\" type=\"web\"/>" };
            var componentItem = new Fixture().Create<ComponentItem>();

            //When
            var result = ComponentManifest.LoadComponentItem(componentItem, "root", mockManifestFileReader.CreateService());

            //Then
            Assert.True(result.IsSuccessful);
        }
    }
}
