using Dewey.Manifest.Repository;
using System.Collections.Generic;

namespace Dewey.State
{
    public class Repository
    {
        private List<Component> _componentList;
        private List<RuntimeResource> _runtimeResourceList;

        public IEnumerable<Component> Components { get { return _componentList; } }
        public IEnumerable<RuntimeResource> RuntimeResources { get { return _runtimeResourceList; } }

        public string Name { get; private set; }

        public Repository(RepositoryManifest manifest)
        {
            Name = manifest.Name;
            _componentList = new List<Component>();
            _runtimeResourceList = new List<RuntimeResource>();
        }

        public void AddComponent(Component component)
        {
            _componentList.Add(component);
        }

        public void AddRuntimeResource(RuntimeResource runtimeResource)
        {
            _runtimeResourceList.Add(runtimeResource);
        }
    }
}
