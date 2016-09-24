using System.Collections.Generic;

namespace Dewey.State
{
    public class RepositoriesFile
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
    }
}
