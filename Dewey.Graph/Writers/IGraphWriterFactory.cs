namespace Dewey.Graph.Writers
{
    public interface IGraphWriterFactory
    {
        IGraphWriter CreateWriter(GraphCommand command);
    }
}
