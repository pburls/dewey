using Dewey.CLI.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dewey.CLI.Repository
{
    public class RepositoryManifest
    {
        public const string DEFAULT_REPOSITORY_FILE_NAME = "repository.xml";

        public static LoadRepositoryItemResult LoadRepositoryItem(RepositoryItem repositoryItem, string rootLocation)
        {
            var repositoryDirectory = new DirectoryInfo(Path.Combine(rootLocation, repositoryItem.RelativeLocation));
            if (!repositoryDirectory.Exists) return LoadRepositoryItemResult.CreateDirectoryNotFoundResult(repositoryDirectory);

            var repositoryManifestFile = new FileInfo(Path.Combine(repositoryDirectory.FullName, DEFAULT_REPOSITORY_FILE_NAME));
            if (!repositoryManifestFile.Exists) return LoadRepositoryItemResult.CreateFileNotFoundResult(repositoryDirectory, repositoryManifestFile);

            var repository = XElement.Load(repositoryManifestFile.FullName);
            var componentsElement = repository.Elements().FirstOrDefault(x => x.Name.LocalName == "components");
            var repositoryManifest = new RepositoryManifest();

            return LoadRepositoryItemResult.CreateSuccessfulResult(repositoryDirectory, repositoryManifestFile, repositoryManifest);
        }
    }
}
