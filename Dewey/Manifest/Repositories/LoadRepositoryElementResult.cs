using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repositories
{
    public class LoadRepositoryElementResult
    {
        public IEnumerable<string> MissingAttributes { get; private set; }
        public RepositoryItem RepositoryItem { get; private set; }
        public XElement RepositoryElement { get; private set; }
        public string ErrorMessage { get; private set; }

        private LoadRepositoryElementResult(XElement repositoryElement, RepositoryItem repositoryItem, IEnumerable<string> missingAttributes)
        {
            if (repositoryElement == null)
            {
                throw new ArgumentNullException("repositoryElement");
            }

            MissingAttributes = missingAttributes;
            RepositoryItem = repositoryItem;
            RepositoryElement = repositoryElement;
            ErrorMessage = GetErrorMessage();
        }

        public static LoadRepositoryElementResult CreateMissingAttributesResult(XElement repositoryElement, IEnumerable<string> missingAttributes)
        {
            return new LoadRepositoryElementResult(repositoryElement, null, missingAttributes);
        }

        public static LoadRepositoryElementResult CreateSuccessfulResult(XElement repositoryElement, RepositoryItem repositoryItem)
        {
            return new LoadRepositoryElementResult(repositoryElement, repositoryItem, null);
        }

        private string GetErrorMessage()
        {
            if (MissingAttributes != null && MissingAttributes.Any())
            {
                return string.Format("Repository element '{0}' is missing the following attributes: {1}", RepositoryElement, string.Join(", ", MissingAttributes));
            }

            return null;
        }
    }
}
