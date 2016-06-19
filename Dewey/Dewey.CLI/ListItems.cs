using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dewey.Manifest.Repositories;

namespace Dewey.CLI
{
    class ListItems
    {
        internal static void WriteList(LoadRepositoriesManifestResult result)
        {
            if (result.RepositoriesManifestFile != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(result.RepositoriesManifestFile.FullName);

                foreach (var repoItem in result.RepositoriesManifest.RepositoryItems)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("├ {0}", repoItem.Name);
                }
            }
        }
    }
}
