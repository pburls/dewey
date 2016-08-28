using Dewey.Messaging;
using SimpleInjector;

namespace Dewey.Manifest
{
    public class Module : IModule
    {
        public Module(Container container, ICommandProcessor commandProcessor)
        {
            var writer = container.GetInstance<LoadManifestFilesWriter>();

            commandProcessor.RegisterHandler<LoadManifestFiles, ManifestLoadHandler>();
        }
    }
}
