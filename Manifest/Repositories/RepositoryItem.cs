using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repositories
{
    public class RepositoryItem
    {
        public string Name { get; private set; }
        public string RelativeLocation { get; private set; }
        public RepositoriesManifest RepositoriesManifest { get; private set; }

        private RepositoryItem(string name, string relativeLocation, RepositoriesManifest repositoriesManifest)
        {
            Name = name;
            RelativeLocation = relativeLocation;
            RepositoriesManifest = repositoriesManifest;
        }

        public static LoadRepositoryElementResult LoadRepositoryElement(XElement repositoryElement, RepositoriesManifest repositoriesManifest)
        {
            var missingAttributes = new List<string>();

            var repoNameAtt = repositoryElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
            if (repoNameAtt == null || string.IsNullOrWhiteSpace(repoNameAtt.Value))
            {
                missingAttributes.Add("name");
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

            var repositoryItem = new RepositoryItem(repoNameAtt.Value, repoLocationAtt.Value, repositoriesManifest);

            return LoadRepositoryElementResult.CreateSuccessfulResult(repositoryElement, repositoryItem);
        }
    }
}
