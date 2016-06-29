using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Manfiest
{
    class XmlFileLoader
    {
        private FileInfo fileInfo;

        public string RelativeFilePath { get; private set; }

        public bool Exists
        {
            get
            {
                return fileInfo.Exists;
            }
        }

        public string FullName
        {
            get
            {
                return fileInfo.FullName;
            }
        }

        public XmlFileLoader(string relativeFilePath)
        {
            RelativeFilePath = relativeFilePath;
            fileInfo = new FileInfo(relativeFilePath);
        }
    }
}
