using Dewey.File;
using System.Xml.Linq;
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

        public IManifestFileReader ReadComponentManifestFile(params string[] paths)
        {
            return _mockManifestFileReader;
        }

        public IManifestFileReader ReadRepositoriesManifestFile()
        {
            return _mockManifestFileReader;
        }

        public IManifestFileReader ReadRepositoryManifestFile(params string[] paths)
        {
            return _mockManifestFileReader;
        }

        public IManifestFileReader ReadRuntimeResourcesManifestFile(params string[] paths)
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

        public string XmlText { get; set; }

        public ManifestFileType MandifestFileType { get; set; }

        public MockManifestFileReader()
        {
            DirectoryExists = true;
            FileExists = true;
        }

        public XElement Load()
        {
            return XElement.Parse(XmlText);
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
