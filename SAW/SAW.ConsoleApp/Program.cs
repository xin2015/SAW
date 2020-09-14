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
            for(int i = 0; i < 10; i++)
            {
                int v = (int)Math.Pow(10, i);
                int r = v / 256 / 256;
                int g = v / 256 % 256;
                int b = v % 256;
                int t = r * 256 * 256 + g * 256 + b;
                if (v == t)
                {
                    Console.WriteLine("==");
                }
                else
                {
                    Console.WriteLine("!=");
                }
            }
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

            //int length = 200;
            //Random rand = new Random();
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //for (int i = 0; i < length; i++)
            //{
            //    int count = 100 + i * 10;
            //    List<int> list = new List<int>();
            //    for (int j = 0; j < count; j++)
            //    {
            //        list.Add(rand.Next(100));
            //    }
            //    SortHelper.BubbleSort(list);
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
    }
}
