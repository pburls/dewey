using System;
using Dewey.Manifest.Repositories;

namespace Dewey.CLI
{
    class BuildAction : ICommand
    {
        public string RepositoryName { get; private set; }
        public string ComponentName { get; private set; }

        BuildAction()
        {

        }

        public static BuildAction Create(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Not enough build action parameters.");
                return null;
            }

            return new BuildAction() { RepositoryName = args[1], ComponentName = args[2] };
        }

        public void Execute(LoadRepositoriesManifestResult loadRepositoriesManifestResult)
        {
            
        }
    }
}
