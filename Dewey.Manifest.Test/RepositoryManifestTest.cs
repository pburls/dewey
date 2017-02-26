using Dewey.File;
using Dewey.Manifest.Repository;
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
    public class RepositoryManifestTest
    {
        [Fact]
        public void RepositoryManifest_LoadRepositoryManifestFile_Should_LoadRuntimeResourceManifestFileElements()
        {
            //Given
            var xmlText =
@"<repository name=""test - repo"">
<runtimeResources>
		<manifestFile name=""ExampleAgent"" location=""ExampleAgent\src"" />
    </runtimeResources>
</repository>";
            var manifestFileReader = new MockManifestFileReader() { MandifestFileType = ManifestFileType.Repository, XmlText = xmlText, DirectoryName = "testLocation" };
            var repositoriesManifest = new Fixture().Create<Repositories.RepositoriesManifest>();

            //When
            var result = RepositoryManifest.LoadRepositoryManifestFile(manifestFileReader, repositoriesManifest);

            //Then
            Assert.True(result.IsSuccessful);
            Assert.NotEmpty(result.RepositoryManifest.RuntimeResourcesItems);
        }
    }
}
