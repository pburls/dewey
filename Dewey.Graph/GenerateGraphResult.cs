using Dewey.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
