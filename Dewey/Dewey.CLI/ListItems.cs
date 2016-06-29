using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using System;
using System.Collections.Generic;

namespace Dewey.CLI
{
    enum ItemColor
    {
        Repositories = ConsoleColor.Cyan,
        RepositoryItem = ConsoleColor.Green,
        ComponentItem = ConsoleColor.Yellow,
    }

    class ListItems : ICommand
    {
        ListItems()
        {

        }

        public static ListItems Create()
        {
            return new ListItems();
        }

        public void Execute(LoadRepositoriesManifestResult result)
        {
            if (result.RepositoriesManifestFile != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(result.RepositoriesManifestFile.FileName);

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

        internal void WriteList(LoadRepositoryItemResult result, List<ItemColor> offsets)
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
