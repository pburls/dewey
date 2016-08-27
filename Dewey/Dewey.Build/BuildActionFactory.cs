using Dewey.Messaging;
using SimpleInjector;
using System;

namespace Dewey.Build
{
    class BuildActionFactory
    {
        public static IBuildAction CreateBuildAction(string buildType, Container container)
        {
            switch (buildType)
            {
                case MSBuild.BUILD_TYPE:
                    return container.GetInstance<MSBuild>();
                default:
                    throw new ArgumentOutOfRangeException("buildType", buildType, string.Format("Unknown build type {0}.", buildType));
            }
        }
    }
}
