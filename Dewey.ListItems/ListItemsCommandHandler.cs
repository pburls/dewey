using Dewey.Messaging;
using Dewey.State.Messages;
using System.Linq;

namespace Dewey.ListItems
{
    public class ListItemsCommandHandler :
        ICommandHandler<ListItemsCommand>,
        IEventHandler<GetRepositoriesFilesResult>,
        IEventHandler<GetRepositoriesResult>,
        IEventHandler<GetComponentsResult>
    {
        readonly ICommandProcessor _commandProcessor;

        public ListItemsCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _commandProcessor = commandProcessor;

            eventAggregator.Subscribe<GetRepositoriesFilesResult>(this);
            eventAggregator.Subscribe<GetRepositoriesResult>(this);
            eventAggregator.Subscribe<GetComponentsResult>(this);
        }

        public void Execute(ListItemsCommand command)
        {
            _commandProcessor.Execute(new GetRepositoriesFiles());
        }

        public void Handle(GetRepositoriesFilesResult getRepositoriesFilesResult)
        {
            if (getRepositoriesFilesResult.RepositoriesFiles.Any())
            {
                foreach (var repositoriesFile in getRepositoriesFilesResult.RepositoriesFiles)
                {
                    repositoriesFile.Write();
                }
            }
            else
            {
                _commandProcessor.Execute(new GetRepositories());
            }
        }

        public void Handle(GetRepositoriesResult getRepositoriesResult)
        {
            if (getRepositoriesResult.Repositories.Any())
            {
                foreach (var repository in getRepositoriesResult.Repositories)
                {
                    repository.Write();
                }
            }
            else
            {
                _commandProcessor.Execute(new GetComponents());
            }
        }

        public void Handle(GetComponentsResult getComponentsResult)
        {
            if (getComponentsResult.Components.Any())
            {
                foreach (var component in getComponentsResult.Components)
                {
                    component.Write();
                }
            }
        }
    }
}
