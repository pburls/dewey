using SimpleInjector;
using System;
using System.Linq;

namespace Dewey.Build
{
    class BuildActionFactory
    {
        public static IBuildAction CreateBuildAction(string buildType, Container container)
        {
            var buildActions = container.GetAllInstances<IBuildAction>().ToDictionary(x => x.BuildType);

            IBuildAction buildAction;
            if(!buildActions.TryGetValue(buildType, out buildAction))
            {
                throw new ArgumentOutOfRangeException("buildType", buildType, string.Format("Unknown build type {0}.", buildType));
            }

            return buildAction;
        }
    }
}
