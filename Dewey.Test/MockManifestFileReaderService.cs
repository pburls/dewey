using Dewey.File;
using System;

namespace Dewey.Test
{
    public class MockManifestFileReaderService : IManifestFileReaderService
    {
        private IManifestFileReader _mockManifestFileReader;

        public MockManifestFileReaderService(IManifestFileReader mockManifestFileReader)
        {
            _mockManifestFileReader = mockManifestFileReader;
        }

        public IManifestFileReader FindManifestFileInCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        public IManifestFileReader ReadDeweyManifestFile(params string[] paths)
        {
            return _mockManifestFileReader;
        }
    }

    public class MockManifestFileReader : IManifestFileReader
    {
        public bool DirectoryExists { get; set; }

        public string DirectoryName { get; set; }

        public bool FileExists { get; set; }

        public string FileName { get; set; }

        public string ScenarioName { get; set; }

        public string Text { get; set; }

        public ManifestFileType MandifestFileType { get; set; }

        public MockManifestFileReader()
        {
            DirectoryExists = true;
            FileExists = true;
        }

        public string LoadText()
        {
            return Text;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(ScenarioName))
            {
                return ScenarioName;
            }
            return base.ToString();
        }

        public MockManifestFileReaderService CreateService()
        {
            return new MockManifestFileReaderService(this);
        }
    }
}
