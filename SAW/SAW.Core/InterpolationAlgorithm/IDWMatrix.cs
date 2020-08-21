using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.InterpolationAlgorithm
{
    public class IDWMatrix
    {
        public double[,] X { get; set; }
        public double[,] Y { get; set; }
        public double[,] T { get; set; }
        public double P { get; set; }

        public IDWMatrix(double[,] X, double[,] Y, double[,] T, double p)
        {
            this.X = X;
            this.Y = Y;
            this.T = T;
            P = -p / 2;
        }

        public double GetValue(double x, double y)
        {
            int iLength = T.GetLength(0), jLength = T.GetLength(1);
            double asum = 0, sum = 0;
            for (int i = 0; i < iLength; i++)
            {
                for (int j = 0; j < jLength; j++)
                {
                    double a = Math.Pow(Math.Pow(X[i, j] - x, 2) + Math.Pow(Y[i, j] - y, 2), P);
                    asum += a;
                    sum += a * T[i, j];
                }
            }
            return sum / asum;
        }
    }
}
