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

    class ListItems : ICommand
    {
    }

    class ListItemsHandler : ICommandHandler<ListItems>, IEventHandler<ComponentManifestLoadResult>, IEventHandler<RepositoryManifestLoadResult>, IEventHandler<RepositoriesManifestLoadResult>
    {
        Dictionary<string, RepositoriesFile> _repositoriesDictionary { get; set; }
        Dictionary<string, Repository> _repositoryDictionary { get; set; }


        public ListItemsHandler(EventAggregator eventAggregator)
        {
            _repositoriesDictionary = new Dictionary<string, RepositoriesFile>();
            _repositoryDictionary = new Dictionary<string, Repository>();

            eventAggregator.Subscribe<RepositoriesManifestLoadResult>(this);
            eventAggregator.Subscribe<RepositoryManifestLoadResult>(this);
            eventAggregator.Subscribe<ComponentManifestLoadResult>(this);
        }

        public void Execute(ListItems command)
        {
            foreach (var repositoriesFile in _repositoriesDictionary.Values)
            {
                repositoriesFile.Write();
            }
        }

        public void Handle(RepositoriesManifestLoadResult repositoriesManifestLoadResult)
        {
            if (repositoriesManifestLoadResult.IsSuccessful)
            {
                RepositoriesFile repositoriesFile = null;
                if (!_repositoriesDictionary.TryGetValue(repositoriesManifestLoadResult.RepositoriesManifest.FileName, out repositoriesFile))
                {
                    repositoriesFile = new RepositoriesFile(repositoriesManifestLoadResult.RepositoriesManifest.FileName);
                    _repositoriesDictionary.Add(repositoriesFile.FileName, repositoriesFile);
                }
            }
        }

        public void Handle(RepositoryManifestLoadResult repositoryManifestLoadResult)
        {
            if (repositoryManifestLoadResult.IsSuccessful)
            {
                RepositoriesFile repositoriesFile = null;
                if (!_repositoriesDictionary.TryGetValue(repositoryManifestLoadResult.RepositoriesManifest.FileName, out repositoriesFile))
                {
                    repositoriesFile = new RepositoriesFile(repositoryManifestLoadResult.RepositoriesManifest.FileName);
                    _repositoriesDictionary.Add(repositoriesFile.FileName, repositoriesFile);
                }

                Repository repository = null;
                if (!_repositoryDictionary.TryGetValue(repositoryManifestLoadResult.RepositoryManifest.Name, out repository))
                {
                    repository = new Repository(repositoryManifestLoadResult.RepositoryManifest.Name);
                    _repositoryDictionary.Add(repository.Name, repository);
                }

                repositoriesFile.AddRepository(repository);
            }
        }

        public void Handle(ComponentManifestLoadResult componentManifestLoadedEvent)
        {
            if (componentManifestLoadedEvent.IsSuccessful)
            {
                Repository repository = null;
                if (!_repositoryDictionary.TryGetValue(componentManifestLoadedEvent.RepositoryManifest.Name, out repository))
                {
                    repository = new Repository(componentManifestLoadedEvent.RepositoryManifest.Name);
                    _repositoryDictionary.Add(repository.Name, repository);
                }

                repository.AddComponent(componentManifestLoadedEvent.ComponentManifest);
            }
        }
    }

    class RepositoriesFile
    {
        private List<Repository> _repositoryList;

        public IEnumerable<Repository> Repositories { get { return _repositoryList; } }

        public string FileName { get; private set; }

        public RepositoriesFile(string fileName)
        {
            FileName = fileName;
            _repositoryList = new List<Repository>();
        }

        public void AddRepository(Repository repository)
        {
            _repositoryList.Add(repository);
        }

        public void Write()
        {
            Console.ForegroundColor = (ConsoleColor)ItemColor.Repositories;
            Console.WriteLine(FileName);

            var offsets = new Stack<ItemColor>();

            foreach (var repository in Repositories)
            {
                repository.Write(offsets);
            }
        }
    }

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

    class Component
    {
        public string Name { get; private set; }

        public Component(ComponentManifest component)
        {
            Name = component.Name;
        }

        public void Write(Stack<ItemColor> offsets)
        {
            offsets.WriteOffsets();

            Console.ForegroundColor = (ConsoleColor)ItemColor.ComponentItem;
            Console.WriteLine("├ {0}", Name);
        }
    }
}
