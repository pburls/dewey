using Dewey.File;
using Dewey.Test;
using Ploeh.AutoFixture;
using Xunit;

namespace Dewey.Manifest.Test.Models
{
    public class ManifestTest
    {
        [Fact]
        public void Manifest_FromJson_Full()
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
    }],
  ""runtimeResources"": [{
      ""type"": ""queue"",
      ""name"": ""incoming"",
      ""provider"": ""ActiveMQ"",
      ""format"": ""XML"",
      ""context"": ""context1""
    }]
}";

            //When
            var result = Dewey.Manifest.Models.Manifest.FromJson(jsonText);

            //Then
            Assert.Single(result.manifestFiles);
            Assert.Single(result.components);
            Assert.Single(result.runtimeResources);
        }

        [Fact]
        public void Manifest_FromJson_JustComponents()
        {
            //Given
            var jsonText =
@"{
  ""components"": [{
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
    }]
}";

            //When
            var result = Dewey.Manifest.Models.Manifest.FromJson(jsonText);

            //Then
            Assert.Single(result.components);
            Assert.Null(result.manifestFiles);
            Assert.Null(result.runtimeResources);
        }

        [Fact]
        public void Manifest_ToJson()
        {
            //Given
            var fixture = new Fixture();
            fixture.Customizations.Add(new PropertyTypeOmitter(typeof(IManifestFileReader)));
            var manifest = fixture.Create<Manifest.Models.Manifest>();

            //When
            var result = manifest.ToJson();

            //Then
            Assert.Contains(manifest.manifestFiles[0].name, result);
            Assert.Contains(manifest.components[0].name, result);
            Assert.Contains(manifest.runtimeResources[0].name, result);
        }

        [Fact]
        public void Manifest_ToJson_JustComponents()
        {
            //Given
            var fixture = new Fixture();
            fixture.Customizations.Add(new PropertyTypeOmitter(typeof(IManifestFileReader)));
            var manifest = fixture.Build<Manifest.Models.Manifest>()
                                  .With(x => x.runtimeResources, null)
                                  .With(x => x.manifestFiles, null)
                                  .Create();

            //When
            var result = manifest.ToJson();

            //Then
            Assert.Contains(manifest.components[0].name, result);
            Assert.DoesNotContain("\"manifestFiles\"", result);
            Assert.DoesNotContain("\"runtimeResources\"", result);
        }
    }
}
