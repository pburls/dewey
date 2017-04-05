using System.Collections.Generic;

namespace Dewey.Build
{
    public class BuildCommandCache : IBuildCommandCache
    {
        readonly List<string> _builtComponentNameList = new List<string>();

        public bool IsComponentAlreadyBuilt(string component)
        {
            if (_builtComponentNameList.Contains(component)) return true;

            _builtComponentNameList.Add(component);
            return false;
        }
    }
}
