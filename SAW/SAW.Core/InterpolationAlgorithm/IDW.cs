﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.InterpolationAlgorithm
{
    public class IDW : IPredict
    {
        public double[] X { get; set; }
        public double[] Y { get; set; }
        public double[] T { get; set; }

        public IDW(double[] X, double[] Y, double[] T)
        {
            this.X = X;
            this.Y = Y;
            this.T = T;
        }

        public double Predict(double x, double y)
        {
            int length = T.Length;
            double asum = 0, sum = 0;
            double xi, yi, a;
            for (int i = 0; i < length; i++)
            {
                xi = X[i] - x;
                yi = Y[i] - y;
                a = 1 / (xi * xi + yi * yi);
                asum += a;
                sum += a * T[i];
            }
            return sum / asum;
        }

        //public double Predict(double x, double y)
        //{
        //    int length = T.Length;
        //    double asum = 0, sum = 0;
        //    double a;
        //    for (int i = 0; i < length; i++)
        //    {
        //        a = 1 / (Math.Pow(X[i] - x, 2) + Math.Pow(Y[i] - y, 2));
        //        asum += a;
        //        sum += a * T[i];
        //    }
        //    return sum / asum;
        //}
    }
}
