using Dewey.Messaging;

namespace Dewey.Graph
{
    public class GenerateGraphResult : IEvent
    {
        public bool IsSuccessful { get; private set; }
        public string GraphFilePath { get; private set; }
        public string ErrorMessage { get; private set; }

        public GenerateGraphResult(bool isSuccessful, string filePath, string errorMessage = null)
        {
            IsSuccessful = isSuccessful;
            GraphFilePath = filePath;
            ErrorMessage = errorMessage;
        }

        public static GenerateGraphResult Create(WriteGraphResult writeGraphResult)
        {
            if (string.IsNullOrEmpty(writeGraphResult.ErrorMessage))
            {
                return new GenerateGraphResult(true, writeGraphResult.FilePath);
            }
            else
            {
                return new GenerateGraphResult(false, writeGraphResult.FilePath, writeGraphResult.ErrorMessage);
            }
        }
    }
}
