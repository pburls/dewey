using System.Collections.Generic;
using Dewey.Deploy.Models;
using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class JsonDeploymentMissingAttributesResult : JsonDeployEvent
    {
        public IEnumerable<string> AttributeNames { get; private set; }

        public JsonDeploymentMissingAttributesResult(Component componentManifest, Models.Deploy deploy, IEnumerable<string> missingAttributes) : base(componentManifest, deploy)
        {
            AttributeNames = missingAttributes;
        }
    }
}