﻿using Ark3.Event;
using Dewey.Manifest.Events;
using System;

namespace Dewey.Manifest
{
    class LoadManifestFilesWriter :
        IEventHandler<LoadManifestFilesStarted>,
        IEventHandler<NoManifestFileFoundResult>,
        IEventHandler<ManifestFilesFound>,
        IEventHandler<ManifestFileNotFound>,
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
