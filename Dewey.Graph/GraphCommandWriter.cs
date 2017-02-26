using Dewey.Messaging;
using System;
using System.Diagnostics;

namespace Dewey.Graph
{
    class GraphCommandWriter :
        IEventHandler<GenerateGraphStarted>,
        IEventHandler<GenerateGraphResult>
    {
        public GraphCommandWriter(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<GenerateGraphStarted>(this);
            eventAggregator.Subscribe<GenerateGraphResult>(this);
        }

        public void Handle(GenerateGraphStarted @event)
        {
            Console.ResetColor();
            Console.WriteLine("Graph generation started.");
        }

        public void Handle(GenerateGraphResult result)
        {
            if (result.IsSuccessful)
            {
                Console.WriteLine("Finished Generating Graph: '{0}'", result.GraphFilePath);

                if (result.GraphCommand.RenderToPNG)
                {
                    Process.Start(result.GraphFilePath);
                }
            }
            else
            {
                Console.WriteLine("Error Generating Graph: '{0}'", result.ErrorMessage);
            }
        }
    }
}
