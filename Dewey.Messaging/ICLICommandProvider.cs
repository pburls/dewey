using Ark3.Command;
using System.Collections.Generic;

namespace Dewey.Messaging
{
    public interface ICLICommandProvider
    {
        IEnumerable<string> CommandWords { get; }

        ICommand CreateCommand(string[] args);
    }
}
