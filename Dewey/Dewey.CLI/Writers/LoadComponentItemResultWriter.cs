using Dewey.Manifest.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI.Writers
{
    public static class LoadComponentItemResultWriter
    {
        public static void WriteErrors(this IEnumerable<LoadComponentItemResult> results)
        {
            foreach (var result in results)
            {
                result.WriteErrors();
            }
        }

        public static void WriteErrors(this LoadComponentItemResult result)
        {
            var errorMessages = new List<string>();

            if (result.ErrorMessage != null)
            {
                errorMessages.Add(result.ErrorMessage);
            }

            if (result.ComponentManifestFile != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(result.ComponentManifestFile.FullName);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var message in errorMessages)
            {
                Console.WriteLine(message);
            }
        }
    }
}
