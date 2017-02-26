using System;
using System.Collections.Generic;

namespace Dewey.ListItems
{
    enum ItemColor
    {
        Repositories = ConsoleColor.Cyan,
        RepositoryItem = ConsoleColor.Green,
        ComponentItem = ConsoleColor.Yellow,
        RuntimeResource = ConsoleColor.DarkCyan,
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
