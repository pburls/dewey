namespace Dewey.Graph.Writers
{
    public class WriteGraphResult
    {
        public string FilePath { get; private set; }
        public string ErrorMessage { get; private set; }

        public WriteGraphResult(string filePath, string errorMessage)
        {
            FilePath = filePath;
            ErrorMessage = errorMessage;
        }
    }
}
