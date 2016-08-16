using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.ListItems
{
    class RepositoriesFile
    {
        private List<Repository> _repositoryList;

        public IEnumerable<Repository> Repositories { get { return _repositoryList; } }

        public string FileName { get; private set; }

        public RepositoriesFile(string fileName)
        {
            FileName = fileName;
            _repositoryList = new List<Repository>();
        }

        public void AddRepository(Repository repository)
        {
            _repositoryList.Add(repository);
        }

        public void Write()
        {
            Console.ForegroundColor = (ConsoleColor)ItemColor.Repositories;
            Console.WriteLine(FileName);

            var offsets = new Stack<ItemColor>();

            foreach (var repository in Repositories)
            {
                repository.Write(offsets);
            }
        }
    }
}
