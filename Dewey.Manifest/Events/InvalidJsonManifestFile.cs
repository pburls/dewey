using Dewey.File;
using System;

namespace Dewey.Manifest.Events
{
    public class InvalidJsonManifestFile : ManifestFileEvent
    {
        public Exception ParseException { get; private set; }
        public InvalidJsonManifestFile(IManifestFileReader manifestFile, Exception ex) : base(manifestFile)
        {
            ParseException = ex;
        }
    }
}
