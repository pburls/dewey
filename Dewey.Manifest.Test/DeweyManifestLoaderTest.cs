using Dewey.Manifest.Events;
using Dewey.Test;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using Xunit;

namespace Dewey.Manifest.Test
{
    public class DeweyManifestLoaderTest
    {
        static IEnumerable<object[]> GetFileNotFoundMockManifestFileReaders()
        {
            yield return new object[] { new MockManifestFileReader() { ScenarioName = "with missing directory", FileExists = false, DirectoryExists = false } };
            yield return new object[] { new MockManifestFileReader() { ScenarioName = "with missing file", FileExists = false, DirectoryExists = true } };
        }

        [Theory]
        [MemberData(nameof(GetFileNotFoundMockManifestFileReaders))]
        public void DeweyManifestLoader_LoadDeweyManifest_returns_ManifestFileNotFound_for(MockManifestFileReader manifestFileReader)
        {
            //When
            var result = DeweyManifestLoader.LoadDeweyManifest(manifestFileReader);

            //Then
            Assert.IsType<ManifestFileNotFound>(result);
        }

        [Fact]
        public void LoadDeweyManifest_returns_InvalidManifestFile_for_manifestFileReader_with_NoRootDeweyManifestElement()
        {
            //Given
            var xmlText =
@"<test/>";
            var manifestFileReader = new MockManifestFileReader() { XmlText = xmlText };

            //When
            var result = DeweyManifestLoader.LoadDeweyManifest(manifestFileReader);

            //Then
            Assert.IsType<InvalidManifestFile>(result);
        }

        [Fact]
        public void DeweyManifestLoader_LoadDeweyManifest_returns_2()
        {
            //Given
            var xmlText =
@"<deweyManifest>
	<repositories>
		<manifestFile name=""test-repo"" location=""test-repo"" />
	</repositories>
</deweyManifest>";
            var manifestFileReader = new MockManifestFileReader() { XmlText = xmlText };
        }
    }
}
