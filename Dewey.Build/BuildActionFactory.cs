using SimpleInjector;
using System;
using System.Linq;

namespace Dewey.Build
{
    public class BuildActionFactory : IBuildActionFactory
    {
        private readonly Container _container;

        public BuildActionFactory(Container container)
        {
            _container = container;
        }

        public IBuildAction CreateBuildAction(string buildType)
        {
            var buildActions = _container.GetAllInstances<IBuildAction>().ToDictionary(x => x.BuildType);

            IBuildAction buildAction;
            if(!buildActions.TryGetValue(buildType, out buildAction))
            {
                throw new ArgumentOutOfRangeException("buildType", buildType, string.Format("Unknown build type {0}.", buildType));
            }

            return buildAction;
        }
    }
}
