﻿using System;
using Dewey.Messaging;

namespace Dewey.Build
{
    public class BuildCommand : ICommand
    {
        public const string COMMAND_TEXT = "build";

        public string ComponentName { get; private set; }

        BuildCommand()
        {

        }

        public static BuildCommand Create(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dewey build <componentName>");
                return null;
            }

            return new BuildCommand() { ComponentName = args[1] };
        }

        public static BuildCommand Create(string componentName)
        {
            return new BuildCommand() { ComponentName = componentName };
        }
    }
}
