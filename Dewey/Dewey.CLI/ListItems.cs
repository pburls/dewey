using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;

namespace Dewey.CLI
{
    enum ItemColor
    {
        Repositories = ConsoleColor.Cyan,
        RepositoryItem = ConsoleColor.Green,
        ComponentItem = ConsoleColor.Yellow,
    }

    static class ListItems
    {
        internal static void WriteList(LoadRepositoriesManifestResult result)
        {
            if (result.RepositoriesManifestFile != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(result.RepositoriesManifestFile.FullName);

                var writeOffsetList = new List<ItemColor>();
                writeOffsetList.Add(ItemColor.RepositoryItem);

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

        internal static void WriteList(LoadRepositoryItemResult result, List<ItemColor> offsets)
        {
            foreach (var compResult in result.LoadComponentElementResults)
            {
                if (compResult.ComponentItem != null)
                {
                    offsets.WriteOffsets();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("├ {0}", compResult.ComponentItem.Name);
                }
            }
        }
    }

    static class ItemColorExtensions
    {
        public static void WriteOffset(this ItemColor color)
        {
            Console.ForegroundColor = (ConsoleColor)color;
            Console.Write("│");
        }

        public static void WriteOffsets(this IEnumerable<ItemColor> colors)
        {
            foreach (var color in colors)
            {
                color.WriteOffset();
            }
        }
    }
}
