using Dewey.Manifest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Dewey.Test
{
    public class ComponentItemTest
    {
        [Fact]
        public void PassingTest()
        {
            XElement componentItemElement = XElement.Parse("<component name=\"ExampleWebApiComp\" location=\"ExampleWebApiComp / \" />");
            //var result = ComponentItem.LoadComponentElement(componentItemElement, "root", mockManifestFileReaderService);
        }
    }
}
