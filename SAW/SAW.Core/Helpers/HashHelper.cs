using SAW.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    public class HashHelper
    {
        public static string ComputeHashSHA1X2String(string path)
        {
            using (SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider())
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    return sha.ComputeHash(fs).ToX2String();
                }
            }
        }
    }
}
