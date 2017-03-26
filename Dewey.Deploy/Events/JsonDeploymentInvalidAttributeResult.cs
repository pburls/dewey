using System.Collections.Generic;
using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class JsonDeploymentInvalidAttributeResult : JsonDeployEvent
    {
        public IEnumerable<string> AttributeNames { get; private set; }

        public JsonDeploymentInvalidAttributeResult(Component componentManifest, Models.Deploy deploy, IEnumerable<string> attributes) : base(componentManifest, deploy)
        {
            AttributeNames = attributes;
        }
    }
}