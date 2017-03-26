using SimpleInjector;

namespace Dewey.File
{
    public class Bootstrapper
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<IFileService, FileService>(Lifestyle.Singleton);
            container.Register<IManifestFileReaderService, ManifestFileReaderService>(Lifestyle.Singleton);
            container.Register<IUserService, UserService>(Lifestyle.Singleton);
        }
    }
}
