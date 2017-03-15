using Dewey.Manifest.Events;
using Dewey.Test;
using Xunit;

namespace Dewey.Manifest.Test
{
    public class DeweyManifestJsonLoaderTest
    {
        [Fact]
        public void LoadJsonDeweyManifest_returns_EmptyManifestFile_for_ManifestFileReader_with_no_text()
        {
            //Given
            var manifestFileReader = new MockManifestFileReader() { Text = @"" };

            //When
            var result = DeweyManifestLoader.LoadJsonDeweyManifest(manifestFileReader);

            //Then
            Assert.IsType<EmptyManifestFile>(result);
        }

        [Fact]
        public void LoadJsonDeweyManifest_returns_JsonManifestLoadResult()
        {
            //Given
            var manifestFileReader = new MockManifestFileReader() { Text = @"{}" };

            //When
            var result = DeweyManifestLoader.LoadJsonDeweyManifest(manifestFileReader);

            //Then
            Assert.IsType<JsonManifestLoadResult>(result);
        }

        [Fact]
        public void LoadJsonDeweyManifest_returns_JsonManifestLoadResult_for_full_json()
        {
            //Given
            var jsonText =
@"{
  ""manifestFiles"": [{
    ""name"": ""ExampleWebApiComp"",
    ""location"": ""ExampleWebApiComp/""
    }],
  ""components"": [{
    ""name"": ""ExampleAgent"",
    ""type"": ""executable"",
    ""sub-type"": ""worker"",
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
    }],
  ""runtimeResources"": [{
      ""type"": ""queue"",
      ""name"": ""incoming"",
      ""provider"": ""ActiveMQ"",
      ""format"": ""XML"",
      ""context"": ""context1""
    }]
}";
            var manifestFileReader = new MockManifestFileReader() { Text = jsonText };

            //When
            var result = DeweyManifestLoader.LoadJsonDeweyManifest(manifestFileReader);

            //Then
            Assert.IsType<JsonManifestLoadResult>(result);
        }
    }
}
