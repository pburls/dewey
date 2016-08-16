using System;
using Dewey.Manifest.Repositories;
using Dewey.Messaging;
using System.Linq;
using Dewey.Manifest;
using Dewey.Manifest.Component;

namespace Dewey.CLI
{
    class BuildCommand : ICommand
    {
        public string ComponentName { get; private set; }

        BuildCommand()
        {

        }

        public static BuildCommand Create(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough build action parameters.");
                return null;
            }

            return new BuildCommand() { ComponentName = args[1] };
        }

        //public void Execute(RepositoriesManifestLoadResult loadRepositoriesManifestResult)
        //{
        //    var loadRepositoryElementResult = loadRepositoriesManifestResult.LoadRepositoryElementResults.FirstOrDefault(result => result.RepositoryItem.Name == RepositoryName);
        //    /*if (loadRepositoryElementResult == null || loadRepositoryElementResult.LoadRepositoryItemResult.RepositoryManifest == null)
        //    {
        //        Console.WriteLine("No Repository with name '{0}' was successfully loaded.", RepositoryName);
        //        return;
        //    }*/

        //    /*var loadComponentElementResult = loadRepositoryElementResult.LoadRepositoryItemResult.LoadComponentElementResults.FirstOrDefault(result => result.ComponentItem.Name == ComponentName);
        //    if (loadComponentElementResult == null || loadComponentElementResult.LoadComponentItemResult.ComponentManifest == null)
        //    {
        //        Console.WriteLine("No Component with name '{0}' was successfully loaded.", ComponentName);
        //        return;
        //    }*/
        //}
    }

    class BuildCommandHandler : ICommandHandler<BuildCommand>,  IEventHandler<ComponentManifestLoadResult>
    {
        readonly ICommandProcessor _commandProcessor;

        BuildCommand _currentBuildCommand;
        ComponentManifest _componentToBuild;

        public BuildCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _commandProcessor = commandProcessor;

            eventAggregator.Subscribe(this);
        }

        public void Execute(BuildCommand command)
        {
            _currentBuildCommand = command;

            _commandProcessor.Execute(new LoadManifestFiles());
        }

        public void Handle(ComponentManifestLoadResult componentManifestLoadResult)
        {
            if (componentManifestLoadResult.IsSuccessful)
            {
                if (componentManifestLoadResult.ComponentManifest.Name == _currentBuildCommand.ComponentName)
                {
                    _componentToBuild = componentManifestLoadResult.ComponentManifest;
                }
            }
        }
    }
}
