using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;

namespace Dewey.CLI
{
    static class ListItems
    {
        static readonly WriteOffset RepositoryItemWriteOffset = new WriteOffset() { ConsoleColor = ConsoleColor.Green, OffsetText = "│" };

        internal static void WriteList(LoadRepositoriesManifestResult result)
        {
            if (result.RepositoriesManifestFile != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(result.RepositoriesManifestFile.FullName);

                var writeOffsetList = new List<WriteOffset>();
                writeOffsetList.Add(RepositoryItemWriteOffset);

                foreach (var repoResult in result.LoadRepositoryElementResults)
                {
                    if (repoResult.RepositoryItem != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("├ {0}", repoResult.RepositoryItem.Name);

                        WriteList(repoResult.LoadRepositoryItemResult, writeOffsetList);
                    }
                }
            }
        }

        private static void WriteList(LoadRepositoryItemResult result, List<WriteOffset> offsets)
        {
            foreach (var compResult in result.LoadComponentElementResults)
            {
                if (compResult.ComponentItem != null)
                {
                    offsets.Write();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("├ {0}", compResult.ComponentItem.Name);
                }
            }
        }
    }

    class WriteOffset
    {
        public string OffsetText { get; set; }
        public ConsoleColor ConsoleColor { get; set; }

        public void Write()
        {
            Console.ForegroundColor = ConsoleColor;
            Console.Write(OffsetText);
        }
    }

    static class WriteOffsetExtensions
    {
        public static void Write(this IEnumerable<WriteOffset> offsets)
        {
            foreach (var offset in offsets)
            {
                offset.Write();
            }
        }
    }
}
