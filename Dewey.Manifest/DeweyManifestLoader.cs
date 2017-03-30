using Dewey.File;
using Dewey.Manifest.Events;
using Newtonsoft.Json;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest
{
    public static class DeweyManifestLoader
    {
        public static ManifestFileEvent LoadJsonDeweyManifest(IManifestFileReader manifestFile)
        {
            if (!manifestFile.DirectoryExists || !manifestFile.FileExists) return new ManifestFileNotFound(manifestFile);

            try
            {
                var manifest = Models.Manifest.FromJson(manifestFile.LoadText());
                return new JsonManifestLoadResult(manifestFile, manifest);
            }
            catch (System.Exception ex)
            {
                return new InvalidJsonManifestFile(manifestFile, ex);
            }
        }
    }


}
