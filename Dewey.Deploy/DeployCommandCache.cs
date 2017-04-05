using System.Collections.Generic;

namespace Dewey.Deploy
{
    class DeployCommandCache : IDeployCommandCache
    {
        readonly List<string> _deployedComponentNameList = new List<string>();
        public bool IsComponentAlreadyDeployed(string component)
        {
            if (_deployedComponentNameList.Contains(component)) return true;

            _deployedComponentNameList.Add(component);
            return false;
        }
    }
}
