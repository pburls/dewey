using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.CLI.Repositories
{
    public class RepositoryItem
    {
        public string Name { get; private set; }
        public string Location { get; private set; }

        private RepositoryItem(string name)
        {
            Name = name;
        }

        public static LoadRepositoryElementResult LoadRepositoryElement(XElement repositoryElement)
        {
            var missingAttributes = new List<string>();
            RepositoryItem repositoryItem = null;

            var repoNameAtt = repositoryElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
            if (repoNameAtt == null || string.IsNullOrWhiteSpace(repoNameAtt.Value))
            {
                missingAttributes.Add(repoNameAtt.Name.LocalName);
            }
            else
            {
                repositoryItem = new RepositoryItem(repoNameAtt.Value);
            }

            var repoLocationAtt = repositoryElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "location");
            if (repoLocationAtt == null || string.IsNullOrWhiteSpace(repoLocationAtt.Value))
            {
                missingAttributes.Add(repoNameAtt.Name.LocalName);
            }

            if (missingAttributes.Any())
            {
                return LoadRepositoryElementResult.CreateMissingAttributesResult(repositoryElement, repositoryItem, missingAttributes);
            }

            repositoryItem.Location = repoLocationAtt.Value;

            return LoadRepositoryElementResult.CreateSuccessfulResult(repositoryElement, repositoryItem);
        }
    }

    public class LoadRepositoryElementResult
    {
        public IEnumerable<string> MissingAttributes { get; private set; }
        public RepositoryItem RepositoryItem { get; private set; }
        public XElement RepositoryElement { get; private set; }
        public string ErrorMessage { get; private set; }

        private LoadRepositoryElementResult(XElement repositoryElement, RepositoryItem repositoryItem, IEnumerable<string> missingAttributes)
        {
            MissingAttributes = missingAttributes;
            RepositoryItem = repositoryItem;
            RepositoryElement = repositoryElement;
            ErrorMessage = GetErrorMessage();
        }

        public static LoadRepositoryElementResult CreateMissingAttributesResult(XElement repositoryElement, RepositoryItem repositoryItem, IEnumerable<string> missingAttributes)
        {
            return new LoadRepositoryElementResult(repositoryElement, repositoryItem, missingAttributes);
        }

        public static LoadRepositoryElementResult CreateSuccessfulResult(XElement repositoryElement, RepositoryItem repositoryItem)
        {
            return new LoadRepositoryElementResult(repositoryElement, repositoryItem, null);
        }

        private string GetErrorMessage()
        {
            if (RepositoryItem == null)
            {
                return "Repository element without a valid name: " + RepositoryElement.ToString();
            }

            if (RepositoryItem != null && MissingAttributes.Any())
            {
                return string.Format("Repository element '{0}' is missing the following attributes: {1}", RepositoryItem.Name, string.Join(", ", MissingAttributes));
            }

            return null;
        }
    }
}
