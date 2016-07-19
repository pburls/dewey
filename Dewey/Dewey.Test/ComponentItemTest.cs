using Dewey.Manifest.Repository;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace Dewey.Test
{
    public class ComponentItemTest
    {
        public class XmlElementData
        {
            public string XmlText { get; set; }
            public string ScenarioName { get; set; }

            public override string ToString()
            {
                return ScenarioName;
            }
        }

        static IEnumerable<object[]> GetUnsuccessfulComponentElements()
        {
            string xmlText = "<component/>";
            yield return new object[] { new XmlElementData() { ScenarioName = "with xml missing all attributes", XmlText = xmlText } };

            xmlText = "<component name=\"ExampleWebApiComp\"/>";
            yield return new object[] { new XmlElementData() { ScenarioName = "with xml missing type attribute", XmlText = xmlText } };

            xmlText = "<component location=\"web\"/>";
            yield return new object[] { new XmlElementData() { ScenarioName = "with xml missing name attribute", XmlText = xmlText } };
        }

        [Theory]
        [MemberData(nameof(GetUnsuccessfulComponentElements))]
        public void LoadComponentElement_returns_UnsuccessfulResult_for(XmlElementData xmlElementData)
        {
            //Given
            XElement componentItemElement = XElement.Parse(xmlElementData.XmlText);

            //When
            var result = ComponentItem.LoadComponentElement(componentItemElement, "root");

            //Then
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void LoadComponentElement_returns_SuccessfulResult_for_complete_component_element()
        {
            //Given
            XElement componentItemElement = XElement.Parse("<component name=\"ExampleWebApiComp\" location=\"ExampleWebApiComp/\" />");

            //When
            var result = ComponentItem.LoadComponentElement(componentItemElement, "root");

            //Then
            Assert.True(result.IsSuccessful);
        }
    }
}
