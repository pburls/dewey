using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using System;
using System.Collections.Generic;

namespace Dewey.ListItems
{
    class Repository
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

        public void Write()
        {
            Console.ForegroundColor = (ConsoleColor)ItemColor.RepositoryItem;
            Console.WriteLine(Name);

            var offsets = new Stack<ItemColor>();

            foreach (var component in Components)
            {
                component.Write(offsets);
            }
        }

        public void Write(Stack<ItemColor> offsets)
        {
            offsets.WriteOffsets();

            Console.ForegroundColor = (ConsoleColor)ItemColor.RepositoryItem;
            Console.WriteLine("├ {0}", Name);

            offsets.Push(ItemColor.RepositoryItem);

            foreach (var component in Components)
            {
                component.Write(offsets);
            }

            offsets.Pop();
        }
    }
}
