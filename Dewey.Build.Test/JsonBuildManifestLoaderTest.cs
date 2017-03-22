using Dewey.Manifest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dewey.Build.Test
{
    public class JsonBuildManifestLoaderTest
    {
        [Fact]
        public void Test1()
        {
            //Given
            var manifestText = 
@"{
    ""name"": ""ExampleAgent"",
    ""type"": ""executable"",
    ""subtype"": ""worker"",
    ""context"": ""context1"",
    ""builds"": [{
      ""type"": ""msbuild"",
      ""target"": ""ExampleAgent.sln"",
      ""msbuildVersion"": ""14.0""
      }],
    ""dependencies"": [{
        ""type"": ""component"",
        ""name"": ""ExampleWebApiComp2""
      }]
}
";
            var componentManifest = Component.FromJson(manifestText);

            //When
            //JsonBuildManifestLoader.LoadFromComponentManifest(componentManifest);
        }
    }
}
