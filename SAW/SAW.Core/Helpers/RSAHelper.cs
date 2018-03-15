using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
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
        public static RSAHelper Default { get; }

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

        static RSAParameters RSAPublicKeyJavaToCSharp(string javaParameters)
        {
            RsaKeyParameters publicKey = (RsaKeyParameters)PublicKeyFactory.CreateKey(javaParameters.FromBase64String());
            return new RSAParameters()
            {
                Modulus = publicKey.Modulus.ToByteArrayUnsigned(),
                Exponent = publicKey.Exponent.ToByteArrayUnsigned()
            };
        }

        static RSAParameters RSAPrivateKeyJavaToCSharp(string javaParameters)
        {
            RsaPrivateCrtKeyParameters privateKey = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(javaParameters.FromBase64String());
            return new RSAParameters()
            {
                Modulus = privateKey.Modulus.ToByteArrayUnsigned(),
                Exponent = privateKey.PublicExponent.ToByteArrayUnsigned(),
                D = privateKey.Exponent.ToByteArrayUnsigned(),
                P = privateKey.P.ToByteArrayUnsigned(),
                Q = privateKey.Q.ToByteArrayUnsigned(),
                DP = privateKey.DP.ToByteArrayUnsigned(),
                DQ = privateKey.DQ.ToByteArrayUnsigned(),
                InverseQ = privateKey.QInv.ToByteArrayUnsigned()
            };
        }

        static RSAHelper()
        {
            Default = new RSAHelper(new RSACryptoServiceProvider());
        }

        RSACryptoServiceProvider _rsa;

        public RSAHelper() : this(new RSACryptoServiceProvider())
        {

        }

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
            return _rsa.ToXmlString(includePrivateParameters);
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

        public byte[] ExportCspBlob(bool includePrivateParameters)
        {
            return _rsa.ExportCspBlob(includePrivateParameters);
        }

        public string ExportBase64CspBlob(bool includePrivateParameters)
        {
            return ExportCspBlob(includePrivateParameters).ToBase64String();
        }

        public void ImportParameters(RSAParameters parameters)
        {
            _rsa.ImportParameters(parameters);
        }

        public void ImportXMLParameters(string xmlParameters)
        {
            _rsa.FromXmlString(xmlParameters);
        }

        public void ImportJavaParameters(string javaParameters, bool includePrivateParameters)
        {
            if (includePrivateParameters)
            {
                ImportParameters(RSAPrivateKeyJavaToCSharp(javaParameters));
            }
            else
            {
                ImportParameters(RSAPublicKeyJavaToCSharp(javaParameters));
            }
        }

        public void ImportCspBlob(byte[] keyBlob)
        {
            _rsa.ImportCspBlob(keyBlob);
        }

        public void ImportBase64CspBlob(string keyBlob)
        {
            ImportCspBlob(keyBlob.FromBase64String());
        }

        public byte[] Decrypt(byte[] data, RSAEncryptionPadding padding)
        {
            return _rsa.Decrypt(data, padding);
        }

        public byte[] Decrypt(byte[] rgb, bool fOAEP)
        {
            return _rsa.Decrypt(rgb, fOAEP);
        }

        /// <summary>
        /// 使用 System.Security.Cryptography.RSA 算法，使用 OAEP 填充，对数据进行解密。
        /// </summary>
        /// <param name="text">Base64格式的加密字符串</param>
        /// <returns>UTF8格式的明文字符串</returns>
        public string Decrypt(string text)
        {
            return Decrypt(text.FromBase64String(), true).ToUTF8String();
        }

        public byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
        {
            return _rsa.Encrypt(data, padding);
        }

        public byte[] Encrypt(byte[] rgb, bool fOAEP)
        {
            return _rsa.Encrypt(rgb, fOAEP);
        }

        /// <summary>
        /// 使用 System.Security.Cryptography.RSA 算法，使用 OAEP 填充，对数据进行加密。
        /// </summary>
        /// <param name="text">UTF8格式的明文字符串</param>
        /// <returns>Base64格式的加密字符串</returns>
        public string Encrypt(string text)
        {
            return Encrypt(text.FromUTF8String(), true).ToBase64String();
        }
    }
}
