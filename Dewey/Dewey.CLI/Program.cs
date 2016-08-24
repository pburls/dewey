using Dewey.Messaging;
using SimpleInjector;
using System;

namespace Dewey.CLI
{
    class Program
    {
        private static ICommand command;

        static void Main(string[] args)
        {
            var container = new Container();

            Bootstrapper.RegisterTypes(container);

            var commandManager = container.GetInstance<CLICommandManager>();

            var moduleCataloge = container.GetInstance<ModuleCatalogue>();
            moduleCataloge.Load<Manifest.Module>();
            moduleCataloge.Load<ListItems.Module>();
            moduleCataloge.Load<Build.Module>();
            moduleCataloge.Load<Deploy.Module>();

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: dewey <command>");
                Console.WriteLine("Commands:");
                foreach (var commandWord in commandManager.CommandWords)
                {
                    Console.WriteLine(" - {0}", commandWord);
                }
                return;
            }

            command = commandManager.CreateCommandForArgs(args);

            if (command == null)
            {
                Console.WriteLine("Unknown command.");
            }
            else
            {
                var commandProcessor = container.GetInstance<ICommandProcessor>();
                var commandHandler = commandProcessor.Execute(command);
                if (commandHandler == null)
                {
                    Console.WriteLine("No command handler registered for command.");
                }
            }

#if DEBUG
            Console.ResetColor();
            Console.WriteLine("Continue...");
            Console.ReadLine();
#endif
        }

        private static void Output(Exception ex)
        {
            Output(ex.Message);
        }

        private static void Output(string message)
        {
            Output(message, null);
        }

        private static void Output(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}
