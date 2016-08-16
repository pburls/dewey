﻿using Dewey.Manifest.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.ListItems
{
    class Component
    {
        public string Name { get; private set; }

        public Component(ComponentManifest component)
        {
            Name = component.Name;
        }

        public void Write(Stack<ItemColor> offsets)
        {
            offsets.WriteOffsets();

            Console.ForegroundColor = (ConsoleColor)ItemColor.ComponentItem;
            Console.WriteLine("├ {0}", Name);
        }
    }
}
