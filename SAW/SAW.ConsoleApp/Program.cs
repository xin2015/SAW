using Common.Logging;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using SAW.Core.Extensions;
using SAW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SAW.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //try
            //{
            //    RSAParameters privateParameter = rsa.ExportParameters(true);
            //    Console.WriteLine(XmlSerializerHelper.SerializeXML<RSAParameters>(privateParameter));
            //    BigInteger modulus = new BigInteger(1, privateParameter.Modulus);
            //    BigInteger publicExponent = new BigInteger(1, privateParameter.Exponent);
            //    BigInteger d = new BigInteger(1, privateParameter.D);
            //    BigInteger p = new BigInteger(1, privateParameter.P);
            //    BigInteger q = new BigInteger(1, privateParameter.Q);
            //    BigInteger dP = new BigInteger(1, privateParameter.DP);
            //    BigInteger dQ = new BigInteger(1, privateParameter.DQ);
            //    BigInteger qInv = new BigInteger(1, privateParameter.InverseQ);
            //    RsaKeyParameters publicKey = new RsaKeyParameters(false, modulus, publicExponent);
            //    RsaPrivateCrtKeyParameters privateCrtKey = new RsaPrivateCrtKeyParameters(modulus, publicExponent, d, p, q, dP, dQ, qInv);
            //    Console.WriteLine(RSAPublicKeyCSharpToJava(publicKey));
            //    Console.WriteLine(rsa.ExportCspBlob(false).ToBase64String());
            //    Console.WriteLine(RSAPrivateKeyCSharpToJava(privateCrtKey));
            //    Console.WriteLine(rsa.ExportCspBlob(true).ToBase64String());
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //rsa.Dispose();

            ILog logger = LogManager.GetLogger<Program>();
            foreach(DriveInfo di in DriveInfo.GetDrives())
            {
                if (di.IsReady)
                {
                    //Console.WriteLine(di.Name);
                    //Console.WriteLine(di.DriveType);
                    //Console.WriteLine(di.DriveFormat);
                    //Console.WriteLine(di.AvailableFreeSpace);
                    //Console.WriteLine(di.TotalFreeSpace);
                    //Console.WriteLine(di.TotalSize);
                    //Console.WriteLine(di.RootDirectory);
                    //Console.WriteLine(di.VolumeLabel);
                    //Console.WriteLine();
                    logger.Debug(di.Name);
                    logger.Debug(di.DriveType);
                    logger.Debug(di.DriveFormat);
                    logger.Debug(di.AvailableFreeSpace);
                    logger.Debug(di.TotalFreeSpace);
                    logger.Debug(di.TotalSize);
                    logger.Debug(di.RootDirectory);
                    logger.Debug(di.VolumeLabel);
                }
            }

            Console.ReadLine();

            
        }


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
    }
}
