namespace Dewey.Graph.Writers
{
    public class GraphWriterFactory : IGraphWriterFactory
    {
        public IGraphWriter CreateWriter(GraphCommand command)
        {
            if (command.RenderToPNG) return new PNGWriter();
            
            return new DOTWriter();
        }
    }
}
