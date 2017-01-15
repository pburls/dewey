using Dewey.Messaging;
using System;
using System.Diagnostics;

namespace Dewey.Graph
{
    class GraphCommandWriter :
        IEventHandler<GenerateGraphResult>
    {
        public GraphCommandWriter(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<GenerateGraphResult>(this);
        }

        public void Handle(GenerateGraphResult result)
        {
            if (result.IsSuccessful)
            {
                Console.WriteLine("Finished Generating Graph: '{0}'", result.GraphFilePath);
                Process.Start(result.GraphFilePath);
            }
            else
            {
                Console.WriteLine("Error Generating Graph: '{0}'", result.ErrorMessage);
            }
        }
    }
}
