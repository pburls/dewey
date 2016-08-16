using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using System.Xml.Linq;

namespace Dewey.Build
{
    interface IBuildAction
    {
        void Build(string rootLocation, XElement buildElement);
    }
}
