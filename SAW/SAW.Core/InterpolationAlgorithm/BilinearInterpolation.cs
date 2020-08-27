using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.InterpolationAlgorithm
{
    public class BilinearInterpolation : IPredict
    {
        public double[] X { get; set; }
        public double[] Y { get; set; }
        public double[] T { get; set; }

        public BilinearInterpolation(double[] X, double[] Y, double[] T)
        {
            this.X = X;
            this.Y = Y;
            this.T = T;
        }

        public double Predict(double x, double y)
        {
            double xa = (x - X[0]) / (X[2] - X[0]);
            double ya = (y - Y[0]) / (Y[2] - Y[0]);
            double xb = 1 - xa;
            double yb = 1 - ya;
            return T[0] * xb * yb + T[1] * xa * yb + T[2] * xa * ya + T[3] * xb * ya;
        }
    }
}
