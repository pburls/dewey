using Dewey.Messaging;
using System;

namespace Dewey.Build
{
    class BuildActionFactory
    {
        public static IBuildAction CreateBuildAction(string buildType, IEventAggregator eventAggregator)
        {
            switch (buildType)
            {
                case MSBuild.BUILD_TYPE:
                    return new MSBuild(eventAggregator);
                default:
                    throw new ArgumentOutOfRangeException("buildType", buildType, string.Format("Unknown build type {0}.", buildType));
            }
        }
    }
}
