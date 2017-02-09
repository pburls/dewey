using System;
using System.Diagnostics;
using System.IO;

namespace Dewey.Graph.Writers
{
    public class PNGWriter : IGraphWriter
    {
        const string GRAPH_VIZ_PATH = @"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe";

        public WriteGraphResult Write(string graphDOTtext)
        {
            if (!File.Exists(GRAPH_VIZ_PATH))
            {
                return new WriteGraphResult(null, string.Format("GraphViz not found at path '{0}'.", GRAPH_VIZ_PATH));
            }

            var graphFileName = "graph.png";

            ProcessStartInfo startInfo = new ProcessStartInfo(GRAPH_VIZ_PATH);
            startInfo.Arguments = "-Tpng -o " + graphFileName;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;

            var process = Process.Start(startInfo);

            using (var stdIn = process.StandardInput)
            {
                stdIn.WriteLine(graphDOTtext);
            }

            process.WaitForExit();

            var graphFileInfo = new FileInfo(graphFileName);
            if (process.ExitCode == 0)
            {
                return new WriteGraphResult(graphFileInfo.FullName, null);
            }

            return new WriteGraphResult(graphFileInfo.FullName, string.Format("GraphViz process exited with existed with error code '{0}'.", process.ExitCode));
        }
    }
}
