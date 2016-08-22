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
                Console.WriteLine("No command paramerter specified.");
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
            
            Console.ResetColor();
            Console.WriteLine("Continue...");
            Console.ReadLine();
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
