using Dewey.Manifest.Events;
using Dewey.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.Manifest
{
    class LoadManifestFilesWriter :
        IEventHandler<LoadManifestFilesStarted>,
        IEventHandler<NoManifestFileFoundResult>,
        IEventHandler<ManifestFilesFound>,
        IEventHandler<ManifestFileNotFound>,
        IEventHandler<InvalidManifestFile>,
        IEventHandler<EmptyManifestFile>,
        IEventHandler<InvalidJsonManifestFile>
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

        public void Handle(InvalidJsonManifestFile @event)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Unable to parse json manifest file '{@event.ManifestFile.FileName}'. {@event.ParseException.Message}");
        }

        public void Handle(ManifestFilesFound @event)
        {
            Console.ResetColor();
            Console.WriteLine("Found manifest file: {0}", @event.FileName);
        }
    }
}
