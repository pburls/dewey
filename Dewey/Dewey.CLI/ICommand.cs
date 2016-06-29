using Dewey.Manifest.Repositories;

namespace Dewey.CLI
{
    interface ICommand
    {
        void Execute(LoadRepositoriesManifestResult loadRepositoriesManifestResult);
    }
}
