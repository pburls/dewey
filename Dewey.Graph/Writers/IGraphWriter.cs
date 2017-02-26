namespace Dewey.Graph.Writers
{
    public interface IGraphWriter
    {
        WriteGraphResult Write(string graphDOTtext);
    }
}
