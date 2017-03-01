using Dewey.Manifest.Component;
using Dewey.Manifest.Events;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using Dewey.Manifest.RuntimeResources;
using Dewey.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.Manifest
{
    class LoadManifestFilesWriter :
        IEventHandler<RepositoriesManifestLoadResult>,
        IEventHandler<RepositoryManifestLoadResult>,
        IEventHandler<ComponentManifestLoadResult>,
        IEventHandler<LoadManifestFilesStarted>,
        IEventHandler<NoManifestFileFoundResult>,
        IEventHandler<ManifestFilesFound>,
        IEventHandler<RuntimeResourcesManifestLoadResult>,
        IEventHandler<ManifestFileNotFound>,
        IEventHandler<InvalidManifestFile>,
        IEventHandler<EmptyManifestFile>
    {
        public LoadManifestFilesWriter(IEventAggregator eventAggregator)
        {
            eventAggregator.SubscribeAll(this);
        }

        public void Handle(LoadManifestFilesStarted @event)
        {
            Console.ResetColor();
            Console.WriteLine("Looking for a manifest file in the current working directory.");
        }

        public void Handle(NoManifestFileFoundResult @event)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Unable to find any manifest file in the current working directory.");
        }

        public void Handle(ManifestFileNotFound @event)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (!@event.ManifestFile.DirectoryExists)
            {
                Console.WriteLine($"Unable to load manifest file '{@event.ManifestFile.FileName}'. Directory does not exist.");
            }
            else
            {
                Console.WriteLine($"Unable to load manifest file '{@event.ManifestFile.FileName}'. File does not exist.");
            }
        }

        public void Handle(EmptyManifestFile @event)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"No dewey xml elements to load in manifest file '{@event.ManifestFile.FileName}'.");
        }

        public void Handle(InvalidManifestFile @event)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Unable to load manifest file '{@event.ManifestFile.FileName}'. The xml file is not a valid manifest.");
        }

        public void Handle(ManifestFilesFound @event)
        {
            Console.ResetColor();
            Console.WriteLine("Found manifest file: {0}", @event.FileName);
        }

        public void Handle(RepositoriesManifestLoadResult @event)
        {
            if (!@event.IsSuccessful || @event.LoadRepositoryElementResults.Any(x => x.ErrorMessage != null))
            {
                var errorMessages = new List<string>();

                if (@event.ErrorMessage != null)
                {
                    errorMessages.Add(@event.ErrorMessage);
                }

                if (@event.LoadRepositoryElementResults != null)
                {
                    errorMessages.AddRange(@event.LoadRepositoryElementResults.Where(x => x.ErrorMessage != null).Select(x => x.ErrorMessage));
                }

                if (@event.RepositoriesManifestFile != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(@event.RepositoriesManifestFile.FileName);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var message in errorMessages)
                {
                    Console.WriteLine(message);
                }
            }
        }

        public void Handle(RepositoryManifestLoadResult @event)
        {
            if (!@event.IsSuccessful || @event.LoadComponentElementResults.Any(x => !x.IsSuccessful))
            {
                var errorMessages = new List<string>();

                if (@event.ErrorMessage != null)
                {
                    errorMessages.Add(@event.ErrorMessage);
                }

                if (@event.LoadComponentElementResults != null)
                {
                    errorMessages.AddRange(@event.LoadComponentElementResults.Where(x => x.ErrorMessage != null).Select(x => x.ErrorMessage));
                }

                if (@event.RepositoryManifestFile != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(@event.RepositoryManifestFile.FileName);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var message in errorMessages)
                {
                    Console.WriteLine(message);
                }
            }
        }

        public void Handle(RuntimeResourcesManifestLoadResult @event)
        {
            if (!@event.IsSuccessful || @event.RuntimeResourceItemLoadResults.Any(x => !x.IsSuccessful))
            {
                var errorMessages = new List<string>();

                if (@event.ErrorMessage != null)
                {
                    errorMessages.Add(@event.ErrorMessage);
                }

                if (@event.RuntimeResourceItemLoadResults != null)
                {
                    errorMessages.AddRange(@event.RuntimeResourceItemLoadResults.Where(x => x.ErrorMessage != null).Select(x => x.ErrorMessage));
                }

                if (@event.ManifestFile != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(@event.ManifestFile.FileName);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var message in errorMessages)
                {
                    Console.WriteLine(message);
                }
            }
        }

        public void Handle(ComponentManifestLoadResult @event)
        {
            if (!@event.IsSuccessful)
            {
                if (@event.ComponentManifestFile != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    if (string.IsNullOrEmpty(@event.ComponentManifestFile.FileName))
                        Console.WriteLine(@event.ComponentManifestFile.DirectoryName);
                    else
                        Console.WriteLine(@event.ComponentManifestFile.FileName);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@event.ErrorMessage);
            }
        }
    }
}
