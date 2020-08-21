using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.InterpolationAlgorithm
{
    public class BilinearInterpolationMatrix
    {
        public double[,] X { get; set; }
        public double[,] Y { get; set; }
        public double[,] T { get; set; }

        public BilinearInterpolationMatrix(double[,] X, double[,] Y, double[,] T)
        {
            this.X = X;
            this.Y = Y;
            this.T = T;
        }

        public double GetValue(double x, double y)
        {
            double xa = (x - X[0, 0]) / (X[1, 1] - X[0, 0]);
            double ya = (y - Y[0, 0]) / (Y[1, 1] - Y[0, 0]);
            double xb = 1 - xa;
            double yb = 1 - ya;
            return T[0, 0] * xb * yb + T[0, 1] * xa * yb + T[1, 1] * xa * ya + T[1, 0] * xb * ya;
        }
    }
}
