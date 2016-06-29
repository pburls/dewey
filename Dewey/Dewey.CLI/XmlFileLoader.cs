using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI
{
    class XmlFileLoader
    {
        private FileInfo fileInfo;

        public string RelativeFilePath { get; private set; }



        public XmlFileLoader(string relativeFilePath)
        {
            RelativeFilePath = relativeFilePath;
            fileInfo = new FileInfo(relativeFilePath);
        }
    }
}
