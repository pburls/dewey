using Dewey.Manifest.Repositories;

namespace Dewey.CLI
{
    interface ICommand
    {
        //void Execute(LoadRepositoriesManifestResult loadRepositoriesManifestResult);
    }

    interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}
