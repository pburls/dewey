using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI.Writers
{
    public static class LoadRepositoriesManifestResultWriter
    {
        public static void Write(this LoadRepositoriesManifestResult result, IReadOnlyDictionary<RepositoryItem, LoadRepositoryItemResult> loadRepositoryItemResults)
        {
            if (result.RepositoriesManifestFile != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(result.RepositoriesManifestFile.FullName);
            }


            if (result.ErrorMessage != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.ErrorMessage);
            }
            else
            {
                foreach (var elementResult in result.LoadRepositoryElementResults)
                {
                    //Console.ForegroundColor = ConsoleColor.Green;
                    //Console.WriteLine(" |");
                    if (elementResult.RepositoryItem == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(string.Format(" |- {0}", elementResult.ErrorMessage));
                    }
                    else
                    {
                        var loadRepositoryItemResult = loadRepositoryItemResults[elementResult.RepositoryItem];
                        loadRepositoryItemResult.Write();
                    }
                }
            }
        }
    }
}
