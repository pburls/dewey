using SimpleInjector;
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

            if (buildActions.ContainsKey(buildType))
            {
                return buildActions[buildType];
            }

            return null;
        }
    }
}
