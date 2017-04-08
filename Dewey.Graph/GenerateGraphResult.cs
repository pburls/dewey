using System;
using Dewey.Messaging;
using Dewey.Graph.Writers;
using Ark3.Command;

namespace Dewey.Graph
{
    public class GenerateGraphResult : ICommandCompleteEvent
    {
        public GraphCommand GraphCommand { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string GraphFilePath { get; private set; }
        public string ErrorMessage { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }

        public ICommand Command { get { return GraphCommand; } }

        public GenerateGraphResult(GraphCommand graphCommand, TimeSpan elapsedTime, bool isSuccessful, string filePath, string errorMessage = null)
        {
            GraphCommand = graphCommand;
            IsSuccessful = isSuccessful;
            GraphFilePath = filePath;
            ElapsedTime = elapsedTime;
            ErrorMessage = errorMessage;
        }

        public static GenerateGraphResult Create(GraphCommand graphCommand, TimeSpan elapsedTime, WriteGraphResult writeGraphResult)
        {
            if (string.IsNullOrEmpty(writeGraphResult.ErrorMessage))
            {
                return new GenerateGraphResult(graphCommand, elapsedTime, true, writeGraphResult.FilePath);
            }
            else
            {
                return new GenerateGraphResult(graphCommand, elapsedTime, false, writeGraphResult.FilePath, writeGraphResult.ErrorMessage);
            }
        }
    }
}
