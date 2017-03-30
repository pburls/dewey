using Dewey.Manifest.Models;
using System;
using System.Collections.Generic;

namespace Dewey.ListItems
{
    static class ComponentExtensions
    {
        public static void Write(this Component component)
        {
            Console.ForegroundColor = (ConsoleColor)ItemColor.ComponentItem;
            Console.WriteLine(component.BuildDescription());
        }

        public static void Write(this Component component, Stack<ItemColor> offsets)
        {
            offsets.WriteOffsets();

            Console.ForegroundColor = (ConsoleColor)ItemColor.ComponentItem;
            Console.WriteLine("├ {0}", component.BuildDescription());
        }

        private static string BuildDescription(this Component component)
        {
            string type = component.type;
            if (!string.IsNullOrWhiteSpace(component.subtype))
            {
                type = string.Format("{0}-{1}", type, component.subtype);
            }

            return string.Format("{0} ({1}) - \"{2}\"", component.name, type, component.File.DirectoryName);
        }
    }
}
