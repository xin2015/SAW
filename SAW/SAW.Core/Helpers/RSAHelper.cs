using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using SAW.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    public class RSAHelper
    {
        static string RSAPublicKeyCSharpToJava(RsaKeyParameters rkp)
        {
            SubjectPublicKeyInfo spki = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(rkp);
            return spki.ToAsn1Object().GetDerEncoded().ToBase64String();
        }

        static string RSAPrivateKeyCSharpToJava(RsaKeyParameters rkp)
        {
            PrivateKeyInfo pki = PrivateKeyInfoFactory.CreatePrivateKeyInfo(rkp);
            return pki.ToAsn1Object().GetEncoded().ToBase64String();
        }

        RSACryptoServiceProvider _rsa;

        public RSAHelper(RSACryptoServiceProvider rsa)
        {
            _rsa = rsa;
        }

        public RSAParameters ExportParameters(bool includePrivateParameters)
        {
            return _rsa.ExportParameters(includePrivateParameters);
        }

        public string ExportXMLParameters(bool includePrivateParameters)
        {
            return XmlSerializerHelper.SerializeXML<RSAParameters>(ExportParameters(includePrivateParameters));
        }

        public byte[] ExportCspBlob(bool includePrivateParameters)
        {
            return _rsa.ExportCspBlob(includePrivateParameters);
        }

        public string ExportBase64CspBlob(bool includePrivateParameters)
        {
            return ExportCspBlob(includePrivateParameters).ToBase64String();
        }

        public string ExportJavaParameters(bool includePrivateParameters)
        {
            RSAParameters parameters = ExportParameters(includePrivateParameters);
            BigInteger modulus = new BigInteger(1, parameters.Modulus);
            BigInteger publicExponent = new BigInteger(1, parameters.Exponent);
            if (includePrivateParameters)
            {
                BigInteger d = new BigInteger(1, parameters.D);
                BigInteger p = new BigInteger(1, parameters.P);
                BigInteger q = new BigInteger(1, parameters.Q);
                BigInteger dP = new BigInteger(1, parameters.DP);
                BigInteger dQ = new BigInteger(1, parameters.DQ);
                BigInteger qInv = new BigInteger(1, parameters.InverseQ);
                RsaPrivateCrtKeyParameters privateCrtKey = new RsaPrivateCrtKeyParameters(modulus, publicExponent, d, p, q, dP, dQ, qInv);
                return RSAPrivateKeyCSharpToJava(privateCrtKey);
            }
            else
            {
                RsaKeyParameters publicKey = new RsaKeyParameters(false, modulus, publicExponent);
                return RSAPublicKeyCSharpToJava(publicKey);
            }
        }
    }
}
