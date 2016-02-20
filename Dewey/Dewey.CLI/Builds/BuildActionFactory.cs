﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI.Builds
{
    class BuildActionFactory
    {
        public static IBuildAction CreateBuildAction(string buildType)
        {
            switch (buildType)
            {
                case "msbuild":
                    return new MSBuild();
                default:
                    throw new ArgumentOutOfRangeException("buildType", buildType, string.Format("Unknown build type {0}.", buildType));
            }
        }
    }
}
