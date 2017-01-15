using Dewey.Manifest.Component;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using Dewey.State.Messages;
using System.Collections.Generic;
using System;
using Dewey.Manifest.RuntimeResources;

namespace Dewey.State
{
    public class Store :
        IEventHandler<RepositoriesManifestLoadResult>, 
        IEventHandler<RepositoryManifestLoadResult>, 
        IEventHandler<ComponentManifestLoadResult>,
        IEventHandler<RuntimeResourcesManifestLoadResult>,
        ICommandHandler<GetRepositoriesFiles>,
        ICommandHandler<GetRepositories>,
        ICommandHandler<GetComponents>,
        ICommandHandler<GetComponent>,
        ICommandHandler<GetRuntimeResources>
    {
        readonly IEventAggregator _eventAggregator;

        Dictionary<string, RepositoriesFile> _repositoriesDictionary { get; set; }
        Dictionary<string, Repository> _repositoryDictionary { get; set; }
        Dictionary<string, Component> _componentsDictionary { get; set; }
        Dictionary<string, RuntimeResource> _runtimeResourceDictionary { get; set; }

        public Store(IEventAggregator eventAggregator, ICommandProcessor commandProcessor)
        {
            _eventAggregator = eventAggregator;
            _repositoriesDictionary = new Dictionary<string, RepositoriesFile>();
            _repositoryDictionary = new Dictionary<string, Repository>();
            _componentsDictionary = new Dictionary<string, Component>();
            _runtimeResourceDictionary = new Dictionary<string, RuntimeResource>();

            commandProcessor.RegisterHandler<GetRepositoriesFiles, Store>();
            commandProcessor.RegisterHandler<GetRepositories, Store>();
            commandProcessor.RegisterHandler<GetComponents, Store>();
            commandProcessor.RegisterHandler<GetComponent, Store>();
            commandProcessor.RegisterHandler<GetRuntimeResources, Store>();

            eventAggregator.Subscribe<RepositoriesManifestLoadResult>(this);
            eventAggregator.Subscribe<ComponentManifestLoadResult>(this);
            eventAggregator.Subscribe<RepositoryManifestLoadResult>(this);
            eventAggregator.Subscribe<RuntimeResourcesManifestLoadResult>(this);
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
                    component = new Component(componentManifestLoadedEvent.ComponentManifest, componentManifestLoadedEvent.ComponentElement);
                    _componentsDictionary.Add(component.ComponentManifest.Name, component);
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

        public void Handle(RuntimeResourcesManifestLoadResult runtimeResourcesManifestLoadResult)
        {
            if (runtimeResourcesManifestLoadResult.IsSuccessful)
            {
                RuntimeResource runtimeResource = null;
                foreach (var runtimeResourceItem in runtimeResourcesManifestLoadResult.RuntimeResourcesManifest.RuntimeResourceItems)
                {
                    if (!_runtimeResourceDictionary.TryGetValue(runtimeResourceItem.Name, out runtimeResource))
                    {
                        runtimeResource = new RuntimeResource(runtimeResourceItem, runtimeResourceItem.Element);
                        _runtimeResourceDictionary.Add(runtimeResourceItem.Name, runtimeResource);
                    }
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

        public void Execute(GetComponent command)
        {
            Component component = null;
            _componentsDictionary.TryGetValue(command.ComponentName, out component);

            _eventAggregator.PublishEvent(new GetComponentResult(command, component));
        }

        public void Execute(GetRuntimeResources command)
        {
            _eventAggregator.PublishEvent(new GetRuntimeResourcesResult(command, _runtimeResourceDictionary.Values));
        }
    }
}
