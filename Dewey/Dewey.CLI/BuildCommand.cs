using System;
using Dewey.Manifest.Repositories;
using System.Linq;

namespace Dewey.CLI
{
    class BuildCommand : ICommand
    {
        public string RepositoryName { get; private set; }
        public string ComponentName { get; private set; }

        BuildCommand()
        {

        }

        public static BuildCommand Create(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Not enough build action parameters.");
                return null;
            }

            return new BuildCommand() { RepositoryName = args[1], ComponentName = args[2] };
        }

        public void Execute(RepositoriesManifestLoadResult loadRepositoriesManifestResult)
        {
            var loadRepositoryElementResult = loadRepositoriesManifestResult.LoadRepositoryElementResults.FirstOrDefault(result => result.RepositoryItem.Name == RepositoryName);
            /*if (loadRepositoryElementResult == null || loadRepositoryElementResult.LoadRepositoryItemResult.RepositoryManifest == null)
            {
                Console.WriteLine("No Repository with name '{0}' was successfully loaded.", RepositoryName);
                return;
            }*/

            /*var loadComponentElementResult = loadRepositoryElementResult.LoadRepositoryItemResult.LoadComponentElementResults.FirstOrDefault(result => result.ComponentItem.Name == ComponentName);
            if (loadComponentElementResult == null || loadComponentElementResult.LoadComponentItemResult.ComponentManifest == null)
            {
                Console.WriteLine("No Component with name '{0}' was successfully loaded.", ComponentName);
                return;
            }*/
        }
    }
}
