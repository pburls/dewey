using Dewey.Messaging;
using System.Collections.Generic;

namespace Dewey.State.Messages
{
    public class GetRepositories : ICommand
    {

    }

    public class GetRepositoriesResult : IEvent
    {
        public GetRepositories Command { get; private set; }
        public IEnumerable<Repository> Repositories { get; private set; }

        public GetRepositoriesResult(GetRepositories command, IEnumerable<Repository> repositories)
        {
            Command = command;
            Repositories = repositories;
        }
    }
}
