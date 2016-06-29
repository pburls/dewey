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
        public static void WriteErrors(this LoadRepositoriesManifestResult result)
        {
            var errorMessages = new List<string>();

            if (result.ErrorMessage != null)
            {
                errorMessages.Add(result.ErrorMessage);
            }

            if (result.LoadRepositoryElementResults != null)
            {
                errorMessages.AddRange(result.LoadRepositoryElementResults.Where(x => x.ErrorMessage != null).Select(x => x.ErrorMessage));
            }

            if (result.RepositoriesManifestFile != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(result.RepositoriesManifestFile.FileName);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var message in errorMessages)
            {
                Console.WriteLine(message);
            }

            result.LoadRepositoryElementResults.Where(x => x.LoadRepositoryItemResult != null).Select(x => x.LoadRepositoryItemResult).WriteErrors();
        }
    }
}
