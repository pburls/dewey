using Dewey.Manifest.Component;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
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
        IEventHandler<LoadManifestFilesStarted>
    {
        public LoadManifestFilesWriter(EventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<RepositoriesManifestLoadResult>(this);
            eventAggregator.Subscribe<RepositoryManifestLoadResult>(this);
            eventAggregator.Subscribe<ComponentManifestLoadResult>(this);
            eventAggregator.Subscribe<LoadManifestFilesStarted>(this);
        }

        public void Handle(LoadManifestFilesStarted @event)
        {
            Console.ResetColor();
            Console.WriteLine("Reading repositories.xml manifest file.");
        }

        public void Handle(RepositoriesManifestLoadResult @event)
        {
            if (!@event.IsSuccessful)
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
            if (!@event.IsSuccessful)
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
