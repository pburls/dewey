using Dewey.Graph.Writers;
using SimpleInjector;

namespace Dewey.Graph
{
    public class Bootstrapper
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<IGraphGenerator, GraphViz>();
            container.Register<IGraphWriterFactory, GraphWriterFactory>();
        }
    }
}
