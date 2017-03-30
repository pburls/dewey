using Dewey.Manifest.Models;
using System.Collections.Generic;

namespace Dewey.Build.Events
{
    public class JsonBuildMissingAttributesResult : JsonBuildEvent
    {
        public IEnumerable<string> AttributeNames { get; private set; }

        public JsonBuildMissingAttributesResult(Component componentManifest, Models.Build build, IEnumerable<string> attributeNames) : base(componentManifest, build)
        {
            AttributeNames = attributeNames;
        }
    }
}
