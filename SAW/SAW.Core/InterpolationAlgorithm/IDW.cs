using System;
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
        public double P { get; set; }

        public IDW(double[] X, double[] Y, double[] T, double p = 2)
        {
            this.X = X;
            this.Y = Y;
            this.T = T;
            P = -p / 2;
        }

        public double Predict(double x, double y)
        {
            int length = T.Length;
            double asum = 0, sum = 0;
            for (int i = 0; i < length; i++)
            {
                double a = Math.Pow(Math.Pow(X[i] - x, 2) + Math.Pow(Y[i] - y, 2), P);
                asum += a;
                sum += a * T[i];
            }
            return sum / asum;
        }
    }
}
