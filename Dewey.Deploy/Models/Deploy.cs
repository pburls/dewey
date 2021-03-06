﻿using Newtonsoft.Json.Linq;
using System;

namespace Dewey.Deploy.Models
{
    public class Deploy : IEquatable<Deploy>
    {
        public JObject BackingData { get; protected set; }
        public string type { get { return (string)BackingData["type"]; } set { BackingData["type"] = value; } }

        public Deploy()
        {
            BackingData = new JObject();
        }

        public Deploy(JObject data)
        {
            BackingData = data;
        }

        public string ToJson()
        {
            return BackingData.ToString();
        }
        public bool Equals(Deploy other)
        {
            if (other == null) return false;

            return type == other.type;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Deploy other = obj as Deploy;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode();
        }

        public static bool operator ==(Deploy a, Deploy b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.type == b.type;
        }

        public static bool operator !=(Deploy a, Deploy b)
        {
            return !(a == b);
        }
    }
}
