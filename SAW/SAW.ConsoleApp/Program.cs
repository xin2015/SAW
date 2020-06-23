using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using SAW.Core.CryptoTransverters;
using SAW.Core.Extensions;
using SAW.Core.Helpers;
using SAW.Core.InterpolationAlgorithm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace SAW.ConsoleApp
{
    class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            Color color = Color.FromArgb(0, 1, 2, 3);
            int argb = color.ToArgb();
            color = Color.FromArgb(argb);

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

            //Console.WriteLine("=> Creating a car and stepping on it!");
            //Car car = new Car("Zippy", 20);
            //try
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        car.Accelerate(10);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("\n*** Error! ***");
            //    Console.WriteLine("Method:{0}", e.TargetSite);
            //    Console.WriteLine("Message:{0}", e.Message);
            //    Console.WriteLine("Source:{0}", e.Source);
            //}

            //Console.WriteLine("\n*** Out of exception logic ***");
            //int length = 200;
            //Random rand = new Random();
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //for (int i = 0; i < length; i++)
            //{
            //    int count = 100 + i * 10;
            //    List<SRSystem> list = new List<SRSystem>();
            //    for (int j = 0; j < count; j++)
            //    {
            //        list.Add(GetSRSystem());
            //    }
            //    list.Sort();
            //}
            //sw.Stop();
            //Console.WriteLine("{0, -20}:{1}", "BubbleSort", sw.Elapsed);
            //sw.Restart();
            //for (int i = 0; i < length; i++)
            //{
            //    int count = 100 + i * 10;
            //    List<int> list = new List<int>();
            //    for (int j = 0; j < count; j++)
            //    {
            //        list.Add(rand.Next(100));
            //    }
            //    SortHelper.BubbleSortWithCheck(list);
            //}
            //sw.Stop();
            //Console.WriteLine("{0, -20}:{1}", "BubbleSortWithCheck", sw.Elapsed);
            //sw.Restart();
            //for (int i = 0; i < length; i++)
            //{
            //    int count = 100 + i * 10;
            //    List<int> list = new List<int>();
            //    for (int j = 0; j < count; j++)
            //    {
            //        list.Add(rand.Next(100));
            //    }
            //    SortHelper.SelectionSort(list);
            //}
            //sw.Stop();
            //Console.WriteLine("{0, -20}:{1}", "SelectionSort", sw.Elapsed);
            //sw.Restart();
            //for (int i = 0; i < length; i++)
            //{
            //    int count = 100 + i * 10;
            //    List<int> list = new List<int>();
            //    for (int j = 0; j < count; j++)
            //    {
            //        list.Add(rand.Next(100));
            //    }
            //    SortHelper.InsertionSort(list);
            //}
            //sw.Stop();
            //Console.WriteLine("{0, -20}:{1}", "InsertionSort", sw.Elapsed);
            //sw.Restart();
            //for (int i = 0; i < length; i++)
            //{
            //    int count = 100 + i * 10;
            //    List<int> list = new List<int>();
            //    for (int j = 0; j < count; j++)
            //    {
            //        list.Add(rand.Next(100));
            //    }
            //    list.Sort();
            //}
            //sw.Stop();
            //Console.WriteLine("{0, -20}:{1}", "Sort", sw.Elapsed);

            //int sampleCount = 5;
            //double[] t = new double[sampleCount];
            //double[] x = new double[sampleCount];
            //double[] y = new double[sampleCount];
            //for (int i = 0; i < 5; i++)
            //{
            //    t[i] = 2 * i + 1;
            //    x[i] = i + 1;
            //    y[i] = i + 1;
            //}
            //Variogram variogram = Kriging.Train(t, x, y, KrigingModel.Exponential, 0, 100);
            //Console.WriteLine(Kriging.Predict(1, 1, variogram));
            //Console.WriteLine(Kriging.Predict(1, 2, variogram));
            //Console.WriteLine(Kriging.Predict(1.5, 1, variogram));
            //Console.WriteLine(Kriging.Predict(1.5, 1.5, variogram));
            //Console.WriteLine(Kriging.Predict(1.5, 2, variogram));
            //Console.WriteLine(Kriging.Predict(2, 2, variogram));
            //Console.ReadLine();



            //Random rand = new Random();
            //int sampleCount = 100;
            //double[][] sources = new double[sampleCount][];
            //double[][] targets = new double[sampleCount][];
            //string[] strings = new string[sampleCount];
            //for (int i = 0; i < sampleCount; i++)
            //{
            //    sources[i] = new double[] { rand.NextDouble() * 180, rand.NextDouble() * 90 };
            //    strings[i] = string.Join(",", sources[i]);
            //}
            //string coords = string.Join(";", strings);
            //string url = string.Format("http://api.map.baidu.com/geoconv/v1/?coords={0}&from=1&to=5&ak=jGMQOfDd4HEYBlqhfsZq4Hj6", coords);
            //using (WebClient wc = new WebClient())
            //{
            //    string text = wc.DownloadString(url);
            //}
            //Console.ReadLine();

        }

        static SRSystem GetSRSystem()
        {
            return new SRSystem()
            {
                Version = string.Format("{0}.{1}.{2}.{3}", rand.Next(20), rand.Next(20), rand.Next(20), rand.Next(20)),
                Price = rand.Next(10000)
            };
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

    class SRSystem
    {
        public string Version { get; set; }
        public int Price { get; set; }
    }
}
