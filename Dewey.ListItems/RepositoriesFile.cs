using Dewey.State;
using System;
using System.Collections.Generic;

namespace Dewey.ListItems
{
    static class RepositoriesFileExtensions
    {
        public static void Write(this RepositoriesFile repositoriesFile)
        {
            Console.ForegroundColor = (ConsoleColor)ItemColor.Repositories;
            Console.WriteLine(repositoriesFile.FileName);

            var offsets = new Stack<ItemColor>();

            foreach (var repository in repositoriesFile.Repositories)
            {
                repository.Write(offsets);
            }
        }
    }
}
