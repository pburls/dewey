using Dewey.Messaging;
using System.Collections.Generic;

namespace Dewey.State.Messages
{
    public class GetRepositoriesFiles : ICommand
    {

    }

    public class GetRepositoriesFilesResult : IEvent
    {
        public GetRepositoriesFiles Command { get; private set; }
        public IEnumerable<RepositoriesFile> RepositoriesFiles { get; private set; }

        public GetRepositoriesFilesResult(GetRepositoriesFiles command, IEnumerable<RepositoriesFile> repositoriesFiles)
        {
            Command = command;
            RepositoriesFiles = repositoriesFiles;
        }
    }
}
