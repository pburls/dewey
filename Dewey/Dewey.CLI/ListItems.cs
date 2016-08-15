using Dewey.Manifest.Component;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using System;
using System.Collections.Generic;

namespace Dewey.CLI
{
    enum ItemColor
    {
        Repositories = ConsoleColor.Cyan,
        RepositoryItem = ConsoleColor.Green,
        ComponentItem = ConsoleColor.Yellow,
    }

    class ListItems : ICommand
    {
        private readonly ListItemState _listItemState;

        public ListItems(EventAggregator eventAggregator)
        {
            _listItemState = new ListItemState();
            eventAggregator.Subscribe<ComponentManifestLoadResult>(_listItemState);
        }

        public void Execute(RepositoriesManifestLoadResult result)
        {
            if (result.RepositoriesManifestFile != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(result.RepositoriesManifestFile.FileName);

                var writeOffsetList = new List<ItemColor>();
                writeOffsetList.Add(ItemColor.RepositoryItem);

                foreach (var repoResult in result.LoadRepositoryElementResults)
                {
                    if (repoResult.RepositoryItem != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("├ {0}", repoResult.RepositoryItem.Name);

                        //WriteList(repoResult.LoadRepositoryItemResult, writeOffsetList);
                    }
                }
            }
        }

        internal void WriteList(RepositoryManifestLoadResult result, List<ItemColor> offsets)
        {
            foreach (var compResult in result.LoadComponentElementResults)
            {
                if (compResult.ComponentItem != null)
                {
                    offsets.WriteOffsets();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("├ {0}", compResult.ComponentItem.Name);
                }
            }
        }
    }

    static class ItemColorExtensions
    {
        public static void WriteOffset(this ItemColor color)
        {
            Console.ForegroundColor = (ConsoleColor)color;
            Console.Write("│");
        }

        public static void WriteOffsets(this IEnumerable<ItemColor> colors)
        {
            foreach (var color in colors)
            {
                color.WriteOffset();
            }
        }
    }

    class ListItemState : IEventHandler<ComponentManifestLoadResult>
    {
        Dictionary<string, Repository> _repositoriesDictionary { get; set; }

        public ListItemState()
        {
            _repositoriesDictionary = new Dictionary<string, Repository>();
        }

        public void Handle(ComponentManifestLoadResult componentManifestLoadedEvent)
        {
            if (componentManifestLoadedEvent.IsSuccessful)
            {
                Repository repository = null;
                if (!_repositoriesDictionary.TryGetValue(componentManifestLoadedEvent.RepositoryManifest.Name, out repository))
                {
                    repository = new Repository(componentManifestLoadedEvent.RepositoryManifest.Name);
                    _repositoriesDictionary.Add(repository.Name, repository);
                }

                repository.AddComponent(componentManifestLoadedEvent.ComponentManifest);
            }
        }
    }

    class Repository
    {
        private List<Component> _componentList;
        public IEnumerable<Component> Components;
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
    }

    class Component
    {
        public string Name { get; private set; }

        public Component(ComponentManifest component)
        {
            Name = component.Name;
        }
    }
}
