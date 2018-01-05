using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.CryptoTransverters
{
    public interface IDecrypt
    {
        byte[] Decrypt(byte[] inputBuffer);
        string Decrypt(string inString);
    }
}
