using Dewey.State;
using System;
using System.Collections.Generic;

namespace Dewey.ListItems
{
    static class ComponentExtensions
    {
        public static void Write(this Component component)
        {
            Console.ForegroundColor = (ConsoleColor)ItemColor.ComponentItem;
            Console.WriteLine("{0} ({1})", component.ComponentManifest.Name, component.ComponentManifest.Type);
        }

        public static void Write(this Component component, Stack<ItemColor> offsets)
        {
            offsets.WriteOffsets();

            Console.ForegroundColor = (ConsoleColor)ItemColor.ComponentItem;
            Console.WriteLine("├ {0} ({1})", component.ComponentManifest.Name, component.ComponentManifest.Type);
        }
    }
}
