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
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
            string a = "[{ \"colname\":[\"序号\",\"范围（单位）\",\"≤100[mg/l]\",\"≤15[mg/l]\",\"6-9[无量纲]\",\"[l/s]\"]},{\"groupheadername\":[{\"numberOfColumns\":1,\"startColumnName\":\"IndexNum\",\"titleText\":\"\"},{\"numberOfColumns\":1,\"startColumnName\":\"DataTime\",\"titleText\":\"监测时间\"},{\"numberOfColumns\":1,\"startColumnName\":\"Z02\",\"titleText\":\"CODcr\"},{\"numberOfColumns\":1,\"startColumnName\":\"Z05\",\"titleText\":\"氨氮\"},{\"numberOfColumns\":1,\"startColumnName\":\"Z60\",\"titleText\":\"pH\"},{\"numberOfColumns\":1,\"startColumnName\":\"Z91\",\"titleText\":\"废水流量\"}]},{\"colmodels\":[{\"align\":\"center\",\"defval\":null,\"editable\":false,\"frozen\":true,\"hidden\":false,\"index\":\"IndexNum\",\"label\":null,\"name\":\"IndexNum\",\"sortable\":false,\"sorttype\":null,\"summaryTpl\":null,\"summaryType\":null,\"title\":false,\"width\":50},{\"align\":\"center\",\"defval\":null,\"editable\":false,\"frozen\":true,\"hidden\":false,\"index\":\"DataTime\",\"label\":null,\"name\":\"DataTime\",\"sortable\":false,\"sorttype\":null,\"summaryTpl\":null,\"summaryType\":null,\"title\":false,\"width\":120},{\"align\":\"center\",\"defval\":\"Z02\",\"editable\":false,\"frozen\":false,\"hidden\":false,\"index\":\"Z02均值\",\"label\":null,\"name\":\"Z02\",\"sortable\":false,\"sorttype\":null,\"summaryTpl\":null,\"summaryType\":null,\"title\":false,\"width\":90},{\"align\":\"center\",\"defval\":\"Z05\",\"editable\":false,\"frozen\":false,\"hidden\":false,\"index\":\"Z05均值\",\"label\":null,\"name\":\"Z05\",\"sortable\":false,\"sorttype\":null,\"summaryTpl\":null,\"summaryType\":null,\"title\":false,\"width\":90},{\"align\":\"center\",\"defval\":\"Z60\",\"editable\":false,\"frozen\":false,\"hidden\":false,\"index\":\"Z60均值\",\"label\":null,\"name\":\"Z60\",\"sortable\":false,\"sorttype\":null,\"summaryTpl\":null,\"summaryType\":null,\"title\":false,\"width\":90},{\"align\":\"center\",\"defval\":\"Z91\",\"editable\":false,\"frozen\":false,\"hidden\":false,\"index\":\"Z91均值\",\"label\":null,\"name\":\"Z91\",\"sortable\":false,\"sorttype\":null,\"summaryTpl\":null,\"summaryType\":null,\"title\":false,\"width\":90}]},{\"DataView\":{\"page\": \"1\", \"total\": \"1\",\"records\":\"1\", \"rows\": [{\"IndexNum\":\"1\",\"DataTime\":\"2019-07-12 10:00\",\"Z02\":\"<span style='text-decoration:none;line-height:24px;' title=''>3.16</span> \",\"Z05\":\"<span style='text-decoration:none;line-height:24px;' title=''>1.01</span> \",\"Z60\":\"<span style='text-decoration:none;line-height:24px;' title=''>7.14</span> \",\"Z91\":\"<span style='text-decoration:none;line-height:24px;' title=''>0.00</span> \"}]}},{\"sgroupheadername\":[{\"numberOfColumns\":1,\"startColumnName\":\"type\",\"titleText\":\"序号\"},{\"numberOfColumns\":1,\"startColumnName\":\"type\",\"titleText\":\"参数类型\"},{\"numberOfColumns\":1,\"startColumnName\":\"Z02\",\"titleText\":\"均值\"},{\"numberOfColumns\":1,\"startColumnName\":\"Z05\",\"titleText\":\"均值\"},{\"numberOfColumns\":1,\"startColumnName\":\"Z60\",\"titleText\":\"均值\"},{\"numberOfColumns\":1,\"startColumnName\":\"Z91\",\"titleText\":\"均值\"}]}]";
            dynamic b = JsonConvert.DeserializeObject(a);
            dynamic c = b[3].DataView.rows[0];
            string d = c.ToString();
            Console.WriteLine(c.DataTime);
            Console.WriteLine(c.Z02);
            Console.WriteLine(c.Z05);
            Console.WriteLine(c.Z60);
            Console.WriteLine(c.Z91);


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

            Console.ReadLine();
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
