using Dewey.Manifest.Dependency;
using Dewey.Messaging;
using Dewey.State.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Graph
{
    public class GraphCommandHandler :
        ICommandHandler<GraphCommand>,
        IEventHandler<GetComponentsResult>,
        IEventHandler<DependencyElementResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly IDependencyElementLoader _dependencyElementLoader;

        readonly List<DependencyElementResult> _dependencies = new List<DependencyElementResult>();

        public GraphCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator, IDependencyElementLoader dependencyElementLoader)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
            _dependencyElementLoader = dependencyElementLoader;

            eventAggregator.Subscribe<GetComponentsResult>(this);
            eventAggregator.Subscribe<DependencyElementResult>(this);
        }

        public void Execute(GraphCommand command)
        {
            _commandProcessor.Execute(new GetComponents());
        }

        public void Handle(GetComponentsResult getComponentsResult)
        {
            foreach (var component in getComponentsResult.Components)
            {
                _dependencyElementLoader.LoadFromComponentManifest(component.ComponentManifest, component.ComponentElement);
            }

            var graphStringBuilder = new StringBuilder();
            foreach (var dependecy in _dependencies)
            {
                graphStringBuilder.AppendLine(string.Format("g.addEdge('{0}', '{1}');", dependecy.ComponentManifest.Name, dependecy.Name));
            }

            var indexFileName = WriteGraphFiles(graphStringBuilder.ToString());

            System.Diagnostics.Process.Start(indexFileName);
        }

        public void Handle(DependencyElementResult dependencyElementResult)
        {
            _dependencies.Add(dependencyElementResult);
        }

        private string WriteGraphFiles(string graph)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var assemblyPath = Path.GetDirectoryName(path);
            var webPath = Path.Combine(assemblyPath, "web");
            var webDirectoryInfo = new DirectoryInfo(webPath);

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var graphDirectoryInfo = Directory.CreateDirectory(string.Format("Graph_{0}", timestamp));

            foreach (var fileInfo in webDirectoryInfo.GetFiles())
            {
                var outputFileName = Path.Combine(graphDirectoryInfo.FullName, fileInfo.Name);
                fileInfo.CopyTo(outputFileName, true);
            }

            var indexFileName = Path.Combine(graphDirectoryInfo.FullName, "index.html");
            string src = File.ReadAllText(indexFileName);
            src = src.Replace("$graphItems$", graph);
            File.WriteAllText(indexFileName, src);

            return indexFileName;
        }
    }
}
