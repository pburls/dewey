using Dewey.Manifest;
using Dewey.Manifest.Component;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using System;
using System.Collections.Generic;

namespace Dewey.ListItems
{
    public class ListItemsCommandHandler :
        ICommandHandler<ListItemsCommand>,
        IEventHandler<ComponentManifestLoadResult>,
        IEventHandler<RepositoryManifestLoadResult>,
        IEventHandler<RepositoriesManifestLoadResult>
    {
        readonly ICommandProcessor _commandProcessor;

        Dictionary<string, RepositoriesFile> _repositoriesDictionary { get; set; }
        Dictionary<string, Repository> _repositoryDictionary { get; set; }
        Dictionary<string, Component> _componentsDictionary { get; set; }

        public ListItemsCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _commandProcessor = commandProcessor;
            _repositoriesDictionary = new Dictionary<string, RepositoriesFile>();
            _repositoryDictionary = new Dictionary<string, Repository>();
            _componentsDictionary = new Dictionary<string, Component>();

            eventAggregator.Subscribe<RepositoriesManifestLoadResult>(this);
            eventAggregator.Subscribe<RepositoryManifestLoadResult>(this);
            eventAggregator.Subscribe<ComponentManifestLoadResult>(this);
        }

        public void Execute(ListItemsCommand command)
        {
            _commandProcessor.Execute(new LoadManifestFiles());

            if (_repositoriesDictionary.Count > 0)
            {
                foreach (var repositoriesFile in _repositoriesDictionary.Values)
                {
                    repositoriesFile.Write();
                }
            }
            else if (_repositoryDictionary.Count > 0)
            {
                foreach (var repository in _repositoryDictionary.Values)
                {
                    repository.Write();
                }
            }
            else if (_componentsDictionary.Count > 0)
            {
                foreach (var component in _componentsDictionary.Values)
                {
                    component.Write();
                }
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
                Repository repository = null;
                if (!_repositoryDictionary.TryGetValue(repositoryManifestLoadResult.RepositoryManifest.Name, out repository))
                {
                    repository = new Repository(repositoryManifestLoadResult.RepositoryManifest);
                    _repositoryDictionary.Add(repository.Name, repository);
                }

                if (repositoryManifestLoadResult.RepositoriesManifest != null)
                {
                    RepositoriesFile repositoriesFile = null;
                    if (!_repositoriesDictionary.TryGetValue(repositoryManifestLoadResult.RepositoriesManifest.FileName, out repositoriesFile))
                    {
                        repositoriesFile = new RepositoriesFile(repositoryManifestLoadResult.RepositoriesManifest.FileName);
                        _repositoriesDictionary.Add(repositoriesFile.FileName, repositoriesFile);
                    }

                    repositoriesFile.AddRepository(repository);
                }
            }
        }

        public void Handle(ComponentManifestLoadResult componentManifestLoadedEvent)
        {
            if (componentManifestLoadedEvent.IsSuccessful)
            {
                Component component = null;
                if (!_componentsDictionary.TryGetValue(componentManifestLoadedEvent.ComponentManifest.Name, out component))
                {
                    component = new Component(componentManifestLoadedEvent.ComponentManifest);
                    _componentsDictionary.Add(component.Name, component);
                }

                if (componentManifestLoadedEvent.RepositoryManifest != null)
                {
                    Repository repository = null;
                    if (!_repositoryDictionary.TryGetValue(componentManifestLoadedEvent.RepositoryManifest.Name, out repository))
                    {
                        repository = new Repository(componentManifestLoadedEvent.RepositoryManifest);
                        _repositoryDictionary.Add(repository.Name, repository);
                    }

                    repository.AddComponent(component);
                }
            }
        }
    }
}
