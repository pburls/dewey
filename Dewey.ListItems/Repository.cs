using Dewey.State;
using System;
using System.Collections.Generic;

namespace Dewey.ListItems
{
    static class RepositoryExtensions
    {
        public static void Write(this Repository repository)
        {
            Console.ForegroundColor = (ConsoleColor)ItemColor.RepositoryItem;
            Console.WriteLine(repository.Name);

            var offsets = new Stack<ItemColor>();

            foreach (var component in repository.Components)
            {
                component.Write(offsets);
            }

            foreach (var runtimeResource in repository.RuntimeResources)
            {
                runtimeResource.Write(offsets);
            }
        }

        public static void Write(this Repository repository, Stack<ItemColor> offsets)
        {
            offsets.WriteOffsets();

            Console.ForegroundColor = (ConsoleColor)ItemColor.RepositoryItem;
            Console.WriteLine("├ {0}", repository.Name);

            offsets.Push(ItemColor.RepositoryItem);

            foreach (var component in repository.Components)
            {
                component.Write(offsets);
            }

            foreach (var runtimeResource in repository.RuntimeResources)
            {
                runtimeResource.Write(offsets);
            }

            offsets.Pop();
        }
    }
}
