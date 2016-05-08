﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dewey.CLI.Repository
{
    public class ComponentItem
    {
        public string Name { get; private set; }
        public string RelativeLocation { get; private set; }

        private ComponentItem(string name)
        {
            Name = name;
        }

        public static LoadComponentElementResult LoadComponentElement(XElement componentElement)
        {
            var missingAttributes = new List<string>();
            ComponentItem componentItem = null;

            var nameAtt = componentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
            if (nameAtt == null || string.IsNullOrWhiteSpace(nameAtt.Value))
            {
                missingAttributes.Add(nameAtt.Name.LocalName);
            }
            else
            {
                componentItem = new ComponentItem(nameAtt.Value);
            }

            var locationAtt = componentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "location");
            if (locationAtt == null || string.IsNullOrWhiteSpace(locationAtt.Value))
            {
                missingAttributes.Add(nameAtt.Name.LocalName);
            }

            if (missingAttributes.Any())
            {
                return LoadComponentElementResult.CreateMissingAttributesResult(componentElement, componentItem, missingAttributes);
            }

            componentItem.RelativeLocation = locationAtt.Value;

            return LoadComponentElementResult.CreateSuccessfulResult(componentElement, componentItem);
        }
    }
}