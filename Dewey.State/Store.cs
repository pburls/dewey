using Dewey.Manifest.Component;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using Dewey.State.Messages;
using System.Collections.Generic;
using System;

namespace Dewey.State
{
    public class Store :
        IEventHandler<RepositoriesManifestLoadResult>, 
        IEventHandler<RepositoryManifestLoadResult>, 
        IEventHandler<ComponentManifestLoadResult>,
        ICommandHandler<GetRepositoriesFiles>,
        ICommandHandler<GetRepositories>,
        ICommandHandler<GetComponents>
    {
        readonly IEventAggregator _eventAggregator;

        Dictionary<string, RepositoriesFile> _repositoriesDictionary { get; set; }
        Dictionary<string, Repository> _repositoryDictionary { get; set; }
        Dictionary<string, Component> _componentsDictionary { get; set; }

        public Store(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _repositoriesDictionary = new Dictionary<string, RepositoriesFile>();
            _repositoryDictionary = new Dictionary<string, Repository>();
            _componentsDictionary = new Dictionary<string, Component>();

            eventAggregator.Subscribe<RepositoriesManifestLoadResult>(this);
            eventAggregator.Subscribe<ComponentManifestLoadResult>(this);
            eventAggregator.Subscribe<RepositoryManifestLoadResult>(this);
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

        public void Execute(GetRepositoriesFiles command)
        {
            _eventAggregator.PublishEvent(new GetRepositoriesFilesResult(command, _repositoriesDictionary.Values));
        }

        public void Execute(GetRepositories command)
        {
            _eventAggregator.PublishEvent(new GetRepositoriesResult(command, _repositoryDictionary.Values));
        }

        public void Execute(GetComponents command)
        {
            _eventAggregator.PublishEvent(new GetComponentsResult(command, _componentsDictionary.Values));
        }
    }
}
