using Dewey.File;
using Dewey.Manifest.RuntimeResources;
using Dewey.Test;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dewey.Manifest.Test
{
    public class RuntimeResourcesManifestLoaderTest
    {
        [Fact]
        public void RuntimeResourcesManifestLoaderTest_LoadRuntimeResourcesManifestFile_Should_LoadRuntimeResourceManifestFileElements()
        {
            //Given
            var xmlText =
@"<runtimeResourcesManifest name=""ExampleAgent"">
	<runtimeResource type=""queue"" name=""incoming"" provider=""ActiveMQ"" context=""context1"" />
</runtimeResourcesManifest>";
            var manifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.RuntimeResources, XmlText = xmlText, DirectoryName = "testLocation" };
            var repositoryManifest = new Fixture().Create<Repository.RepositoryManifest>();

            //When
            var result = RuntimeResourcesManifestLoader.LoadRuntimeResourcesManifestFile(manifestFileReader, repositoryManifest);

            //Then
            Assert.True(result.IsSuccessful);
            Assert.NotEmpty(result.RuntimeResourcesManifest.RuntimeResourceItems);
        }


        [Fact]
        public void RuntimeResourcesManifestLoaderTest_LoadRuntimeResourcesManifestFile_Should_BeSuccesful_WithMissingProviderAndContext()
        {
            //Given
            var xmlText =
@"<runtimeResourcesManifest name=""ExampleAgent"">
	<runtimeResource type=""queue"" name=""incoming"" />
</runtimeResourcesManifest>";
            var manifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.RuntimeResources, XmlText = xmlText, DirectoryName = "testLocation" };
            var repositoryManifest = new Fixture().Create<Repository.RepositoryManifest>();

            //When
            var result = RuntimeResourcesManifestLoader.LoadRuntimeResourcesManifestFile(manifestFileReader, repositoryManifest);

            //Then
            Assert.True(result.IsSuccessful);
            Assert.NotEmpty(result.RuntimeResourcesManifest.RuntimeResourceItems);
        }
    }
}
