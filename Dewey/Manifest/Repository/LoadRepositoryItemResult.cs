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

        public DirectoryInfo RepositoryDirectory { get; private set; }

        public FileInfo RepositoryManifestFile { get; private set; }

        public RepositoryManifest RepositoryManifest { get; private set; }

        public IEnumerable<LoadComponentElementResult> LoadComponentElementResults { get; private set; }

        public string ErrorMessage { get; private set; }

        private LoadRepositoryItemResult(RepositoryItem repositoryItem, DirectoryInfo repositoryDirectory, FileInfo repositoryManifestFile, RepositoryManifest repositoryManifest, IEnumerable<LoadComponentElementResult> loadComponentElementResults)
        {
            if (repositoryItem == null)
            {
                throw new ArgumentNullException("repositoryItem");
            }

            RepositoryItem = repositoryItem;
            RepositoryDirectory = repositoryDirectory;
            RepositoryManifestFile = repositoryManifestFile;
            RepositoryManifest = repositoryManifest;
            LoadComponentElementResults = loadComponentElementResults;
            ErrorMessage = GetErrorMessage();
        }

        public static LoadRepositoryItemResult CreateDirectoryNotFoundResult(RepositoryItem repositoryItem, DirectoryInfo repositoryDirectory)
        {
            return new LoadRepositoryItemResult(repositoryItem, repositoryDirectory, null, null, null);
        }

        public static LoadRepositoryItemResult CreateFileNotFoundResult(RepositoryItem repositoryItem, DirectoryInfo repositoryDirectory, FileInfo repositoryManifestFile)
        {
            return new LoadRepositoryItemResult(repositoryItem, repositoryDirectory, repositoryManifestFile, null, null);
        }

        public static LoadRepositoryItemResult CreateSuccessfulResult(RepositoryItem repositoryItem, DirectoryInfo repositoryDirectory, FileInfo repositoryManifestFile, RepositoryManifest repositoryManifest, IEnumerable<LoadComponentElementResult> loadComponentElementResults)
        {
            return new LoadRepositoryItemResult(repositoryItem, repositoryDirectory, repositoryManifestFile, repositoryManifest, loadComponentElementResults);
        }

        private string GetErrorMessage()
        {
            if (!RepositoryDirectory.Exists) return string.Format("Respository directory '{0}' not found.", RepositoryDirectory.FullName);
            if (!RepositoryManifestFile.Exists) return string.Format("Repository Manifest file '{0}' not found.", RepositoryManifestFile.FullName);

            return null;
        }
    }
}
