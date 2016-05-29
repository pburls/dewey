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
        public static void Write(this LoadRepositoryItemResult result)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(string.Format(" |- {0}", result.RepositoryItem.Name));

            if (result.RepositoryManifestFile != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(string.Format(" |  {0}", result.RepositoryManifestFile.FullName));
            }

            if (result.ErrorMessage != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.ErrorMessage);
            }
            else
            {
                foreach (var elementResult in result.LoadComponentElementResults)
                {
                    if (elementResult.ComponentItem == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(string.Format("    |- {0}", elementResult.ErrorMessage));
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(string.Format("    |- {0}", elementResult.ComponentItem.Name));
                    }
                }
            }
        }
    }
}
