using Dewey.Manifest.Component;
using System;
using System.Collections.Generic;

namespace Dewey.ListItems
{
    class Repository
    {
        private List<Component> _componentList;

        public IEnumerable<Component> Components { get { return _componentList; } }

        public string Name { get; private set; }

        public Repository(string name)
        {
            Name = name;
            _componentList = new List<Component>();
        }

        public void AddComponent(ComponentManifest component)
        {
            _componentList.Add(new Component(component));
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
