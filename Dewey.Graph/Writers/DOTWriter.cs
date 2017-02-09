using System;
using System.IO;

namespace Dewey.Graph.Writers
{
    public class DOTWriter : IGraphWriter
    {
        public WriteGraphResult Write(string graphDOTtext)
        {
            var graphFileName = "graph.gv";
            File.WriteAllText(graphFileName, graphDOTtext);
            var graphFileInfo = new FileInfo(graphFileName);
            return new WriteGraphResult(graphFileInfo.FullName, null);
        }
    }
}
