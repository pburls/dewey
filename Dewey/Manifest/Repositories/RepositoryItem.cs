using Dewey.Manfiest;
using Dewey.Manifest.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repositories
{
    public class RepositoryItem
    {
        public string Name { get; private set; }
        public string RelativeLocation { get; private set; }

        private RepositoryItem(string name)
        {
            Name = name;
        }

        public static LoadRepositoryElementResult LoadRepositoryElement(XElement repositoryElement, string respositoriesRoot, IManifestFileReaderService manifestFileReaderService)
        {
            var missingAttributes = new List<string>();
            RepositoryItem repositoryItem = null;

            var repoNameAtt = repositoryElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
            if (repoNameAtt == null || string.IsNullOrWhiteSpace(repoNameAtt.Value))
            {
                missingAttributes.Add("name");
            }
            else
            {
                repositoryItem = new RepositoryItem(repoNameAtt.Value);
            }

            var repoLocationAtt = repositoryElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "location");
            if (repoLocationAtt == null || string.IsNullOrWhiteSpace(repoLocationAtt.Value))
            {
                missingAttributes.Add("location");
            }

            if (missingAttributes.Any())
            {
                return LoadRepositoryElementResult.CreateMissingAttributesResult(repositoryElement, missingAttributes);
            }

            repositoryItem.RelativeLocation = repoLocationAtt.Value;

            var loadRepositoryItemResult = RepositoryManifest.LoadRepositoryItem(repositoryItem, respositoriesRoot, manifestFileReaderService);

            return LoadRepositoryElementResult.CreateSuccessfulResult(repositoryElement, repositoryItem, loadRepositoryItemResult);
        }
    }
}
