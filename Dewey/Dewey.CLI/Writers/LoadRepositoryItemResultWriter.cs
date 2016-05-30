using Dewey.Manifest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI.Writers
{
    public static class LoadRepositoryItemResultWriter
    {
        public static void WriteErrors(this IEnumerable<LoadRepositoryItemResult> results)
        {
            foreach (var result in results)
            {
                result.WriteErrors();
            }
        }

        public static void WriteErrors(this LoadRepositoryItemResult result)
        {
            var errorMessages = new List<string>();

            if (result.ErrorMessage != null)
            {
                errorMessages.Add(result.ErrorMessage);
            }

            if (result.LoadComponentElementResults != null)
            {
                errorMessages.AddRange(result.LoadComponentElementResults.Where(x => x.ErrorMessage != null).Select(x => x.ErrorMessage));
            }

            if (result.RepositoryManifestFile != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result.RepositoryManifestFile.FullName);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var message in errorMessages)
            {
                Console.WriteLine(message);
            }
        }
    }
}
