using Dewey.Build;
using Dewey.Deploy;
using Dewey.ListItems;
using Dewey.Manifest;
using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.CLI
{
    delegate void ComponentAction(ComponentItem repoComponent, ComponentManifest componentManifest, XElement componentElement);

    class Program
    {
        private static ICommand command;

        static void Main(string[] args)
        {
            var eventAggregator = new EventAggregator();
            var manifestLoadResultErrorWriter = new LoadManifestFilesWriter(eventAggregator);
            var buildCommandWriter = new BuildCommandWriter(eventAggregator);
            var deployCommandWriter = new DeployCommandWriter(eventAggregator);

            var commandProcessor = new CommandProcessor(eventAggregator);
            commandProcessor.RegisterHandler<LoadManifestFiles, ManifestLoadHandler>();
            commandProcessor.RegisterHandler<ListItemsCommand, ListItemsCommandHandler>();
            commandProcessor.RegisterHandler<BuildCommand, BuildCommandHandler>();
            commandProcessor.RegisterHandler<DeployCommand, DeployCommandHandler>();

            if (args.Length < 1)
            {
                Console.WriteLine("No action paramerter specified.");
                return;
            }

            switch (args[0])
            {
                case BuildCommand.COMMAND_TEXT:
                    command = BuildCommand.Create(args);
                    break;
                case DeployCommand.COMMAND_TEXT:
                    command = DeployCommand.Create(args);
                    break;
                case ListItemsCommand.COMMAND_TEXT:
                    command = ListItemsCommand.Create(args);
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    return;
            }

            if (command == null)
                goto done;

            var commandHandler = commandProcessor.Execute(command);
            if (commandHandler == null)
            {
                Console.WriteLine("No command handler registered for command.");
            }

            done:
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
