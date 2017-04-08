using Ark3.Command;
using System;
using System.Linq;
using System.Text;

namespace Dewey.Graph
{
    public class GraphCommand : ICommand
    {
        public const string COMMAND_TEXT = "graph";

        public bool RenderToPNG { get; private set; }

        public GraphCommand(bool renderToPNG)
        {
            RenderToPNG = renderToPNG;
        }

        public static GraphCommand Create(string[] args)
        {
            var arguments = args.Skip(1);
            var switches = arguments.Where(arg => arg.StartsWith("-"));

            if (switches.Any(s => s.Contains("h")))
            {
                Console.WriteLine("Usage: dewey graph [switches]");
                Console.WriteLine("Switches:");
                Console.WriteLine(" -r     : Uses graphviz dot to [r]ender the generated dot graph as a png file.");
                return null;
            }
            var renderToPNG = switches.Any(s => s.Contains("r"));

            return new GraphCommand(renderToPNG);
        }

        public override string ToString()
        {
            var switchesBuilder = new StringBuilder();
            if (RenderToPNG) switchesBuilder.Append(" -r");

            return string.Format("{0}{1}", COMMAND_TEXT, switchesBuilder.ToString());
        }
    }
}
