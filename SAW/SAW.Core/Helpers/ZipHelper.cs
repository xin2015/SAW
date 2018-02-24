using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    public class ZipHelper
    {
        public static void ExtractAll(string fileName, string path)
        {
            using (ZipFile zf = new ZipFile(fileName))
            {
                zf.ExtractAll(path);
            }
        }

        public static void ExtractAll(string fileName, string path, ExtractExistingFileAction extractExistingFile)
        {
            using (ZipFile zf = new ZipFile(fileName))
            {
                zf.ExtractAll(path, extractExistingFile);
            }
        }

        public static void CompressAll(string fileName, string path)
        {
            using (ZipFile zf = new ZipFile())
            {
                zf.AddDirectory(path);
                zf.Save(fileName);
            }
        }
    }
}
