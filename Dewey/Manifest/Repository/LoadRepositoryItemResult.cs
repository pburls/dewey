using Dewey.Manfiest;
using Dewey.Manifest.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dewey.Manifest.Repository
{
    public class LoadRepositoryItemResult
    {
        public RepositoryItem RepositoryItem { get; private set; }

        public XmlFileLoader RepositoryManifestFile { get; private set; }

        public RepositoryManifest RepositoryManifest { get; private set; }

        public IEnumerable<LoadComponentElementResult> LoadComponentElementResults { get; private set; }

        public string ErrorMessage { get; private set; }

        private LoadRepositoryItemResult(RepositoryItem repositoryItem, XmlFileLoader repositoryManifestFile, RepositoryManifest repositoryManifest, IEnumerable<LoadComponentElementResult> loadComponentElementResults)
        {
            if (repositoryItem == null)
            {
                throw new ArgumentNullException("repositoryItem");
            }

            RepositoryItem = repositoryItem;
            RepositoryManifestFile = repositoryManifestFile;
            RepositoryManifest = repositoryManifest;
            LoadComponentElementResults = loadComponentElementResults ?? new List<LoadComponentElementResult>();
            ErrorMessage = GetErrorMessage();
        }

        public static LoadRepositoryItemResult CreateFileNotFoundResult(RepositoryItem repositoryItem, XmlFileLoader repositoryManifestFile)
        {
            return new LoadRepositoryItemResult(repositoryItem, repositoryManifestFile, null, null);
        }

        public static LoadRepositoryItemResult CreateSuccessfulResult(RepositoryItem repositoryItem, XmlFileLoader repositoryManifestFile, RepositoryManifest repositoryManifest, IEnumerable<LoadComponentElementResult> loadComponentElementResults)
        {
            return new LoadRepositoryItemResult(repositoryItem, repositoryManifestFile, repositoryManifest, loadComponentElementResults);
        }

        private string GetErrorMessage()
        {
            if (!RepositoryManifestFile.DirectoryExists) return string.Format("Respository directory '{0}' not found.", RepositoryManifestFile.DirectoryName);
            if (!RepositoryManifestFile.FileExists) return string.Format("Repository Manifest file '{0}' not found.", RepositoryManifestFile.FileName);

            return null;
        }
    }
}
