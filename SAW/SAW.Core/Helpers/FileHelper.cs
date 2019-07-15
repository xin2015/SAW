using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    public class FileHelper
    {
        public void Rename(string directory, string source, string dest)
        {
            string[] files = Directory.GetFiles(directory);
            foreach (string sourceFile in files)
            {
                string destFile = sourceFile.Replace(source, dest);
                File.Move(sourceFile, destFile);
            }
        }
    }
}
