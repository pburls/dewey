﻿using Dewey.Manifest;
using Dewey.Messaging;
using SimpleInjector;
using System;
using System.Diagnostics;

namespace Dewey.CLI
{
    class Program
    {
        private static ICommand command;

        static void Main(string[] args)
        {
            var container = new Container();

            Bootstrapper.RegisterTypes(container);
            File.Bootstrapper.RegisterTypes(container);
            State.Bootstrapper.RegisterTypes(container);
            Build.Bootstrapper.RegisterTypes(container);

            var commandManager = container.GetInstance<CLICommandManager>();

            var moduleCataloge = container.GetInstance<ModuleCatalogue>();
            moduleCataloge.Load<State.Module>();
            moduleCataloge.Load<Manifest.Module>();
            moduleCataloge.Load<ListItems.Module>();
            moduleCataloge.Load<Build.Module>();
            moduleCataloge.Load<Deploy.Module>();

            if (args.Length < 1)
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                Console.WriteLine("Dewey Development Tool. v{0}", version);
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
                var eventAggregator = container.GetInstance<IEventAggregator>();
                var commandReport = new CommandReport(command, eventAggregator);

                //Load Manifest Files first to create a store to use.
                commandProcessor.Execute(new LoadManifestFiles());

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
    }
}
