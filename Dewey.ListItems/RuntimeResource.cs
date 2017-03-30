using Dewey.Manifest.Models;
using System;
using System.Collections.Generic;

namespace Dewey.ListItems
{
    static class RuntimeResourceExtensions
    {
        public static void Write(this RuntimeResource runtimeResource)
        {
            Console.ForegroundColor = (ConsoleColor)ItemColor.RuntimeResource;
            Console.WriteLine(runtimeResource.BuildDescription());
        }

        public static void Write(this RuntimeResource runtimeResource, Stack<ItemColor> offsets)
        {
            offsets.WriteOffsets();

            Console.ForegroundColor = (ConsoleColor)ItemColor.RuntimeResource;
            Console.WriteLine("├ {0}", runtimeResource.BuildDescription());
        }

        private static string BuildDescription(this RuntimeResource runtimeResource)
        {
            return string.Format("{0} ({1}) - \"{2}\"", runtimeResource.name, runtimeResource.type, runtimeResource.File.DirectoryName);
        }
    }
}
