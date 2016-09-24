using Dewey.Manifest.Repository;
using System.Collections.Generic;

namespace Dewey.State
{
    public class Repository
    {
        private List<Component> _componentList;

        public IEnumerable<Component> Components { get { return _componentList; } }

        public string Name { get; private set; }

        public Repository(RepositoryManifest manifest)
        {
            Name = manifest.Name;
            _componentList = new List<Component>();
        }

        public void AddComponent(Component component)
        {
            _componentList.Add(component);
        }
    }
}
