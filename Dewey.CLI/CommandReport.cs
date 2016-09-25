using Dewey.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.CLI
{
    public class CommandReport : IEventHandler<ICommandCompleteEvent>
    {
        readonly List<ICommandCompleteEvent> _completedCommands = new List<ICommandCompleteEvent>();
        readonly ICommand _command;

        public CommandReport(ICommand command, IEventAggregator eventAggregator)
        {
            _command = command;

            eventAggregator.Subscribe(this);
        }

        public void Handle(ICommandCompleteEvent commandCompleteEvent)
        {
            _completedCommands.Add(commandCompleteEvent);

            if (_command == commandCompleteEvent.Command)
            {
                WriteCommandReport();
            }
        }

        private void WriteCommandReport()
        {
            if (_completedCommands.Count == 1)
            {
                if (_completedCommands.All(x => x.IsSuccessful))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Command completed succesfully.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Command FAILED.");
                }
            }
            else
            {
                var successCount = _completedCommands.Where(x => x.IsSuccessful).Count();
                if (_completedCommands.All(x => x.IsSuccessful))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Command completed succesfully. {0} of {1} commands completed successfully.", successCount, _completedCommands.Count);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Command FAILED. {0} of {1} commands completed successfully.", successCount, _completedCommands.Count);
                }
            }

            foreach (var commandCompleteEvent in _completedCommands)
            {
                if (commandCompleteEvent.IsSuccessful)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("- ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("X ");
                }

                Console.WriteLine("{0} : {1}", commandCompleteEvent.ElapsedTime, commandCompleteEvent.Command);
            }
        }
    }
}
