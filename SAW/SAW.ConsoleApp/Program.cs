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

            //ILog logger = LogManager.GetLogger<Program>();
            //foreach (DriveInfo di in DriveInfo.GetDrives())
            //{
            //    if (di.IsReady)
            //    {
            //        Console.WriteLine(di.Name);
            //        Console.WriteLine(di.DriveType);
            //        Console.WriteLine(di.DriveFormat);
            //        Console.WriteLine(di.AvailableFreeSpace);
            //        Console.WriteLine(di.TotalFreeSpace);
            //        Console.WriteLine(di.TotalSize);
            //        Console.WriteLine(di.RootDirectory);
            //        Console.WriteLine(di.VolumeLabel);
            //        Console.WriteLine();
            //    }
            //}

            Console.WriteLine("=> Creating a car and stepping on it!");
            Car car = new Car("Zippy", 20);
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    car.Accelerate(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n*** Error! ***");
                Console.WriteLine("Method:{0}", e.TargetSite);
                Console.WriteLine("Message:{0}", e.Message);
                Console.WriteLine("Source:{0}", e.Source);
            }

            Console.WriteLine("\n*** Out of exception logic ***");
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

    class Car
    {
        public const int MaxSpeed = 100;
        public int CurrentSpeed { get; set; }
        public string PetName { get; set; }

        private bool carIsDead;

        public Car() { }
        public Car(string name, int speed)
        {
            CurrentSpeed = speed;
            PetName = name;
        }

        public void Accelerate(int delta)
        {
            if (carIsDead)
            {
                Console.WriteLine("{0} is out of order...", PetName);
            }
            else
            {
                CurrentSpeed += delta;
                if (CurrentSpeed > MaxSpeed)
                {
                    CurrentSpeed = 0;
                    carIsDead = true;

                    throw new Exception(string.Format("{0} has overheated!", PetName));
                }
                else
                {
                    Console.WriteLine("=> CurrentSpeed = {0}", CurrentSpeed);
                }
            }
        }
    }
}
