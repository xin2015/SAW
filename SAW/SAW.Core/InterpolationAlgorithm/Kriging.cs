using SAW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.InterpolationAlgorithm
{
    public class Kriging : IPredict
    {
        public double[] X { get; set; }
        public double[] Y { get; set; }
        public double[] T { get; set; }
        public double Nugget { get; set; }
        public double Range { get; set; }
        public double Sill { get; set; }
        public double A { get; set; }
        public Func<double, double> Model { get; set; }
        public double[] M { get; set; }

        public double[] K { get; set; }

        public Kriging(double[] X, double[] Y, double[] T, double a = 1.0 / 3)
        {
            this.X = X;
            this.Y = Y;
            this.T = T;
            A = a;
        }

        public double KrigingVariogramGaussian(double h)
        {
            return Nugget + ((Sill - Nugget) / Range) * (1.0 - Math.Exp(-(1.0 / A) * Math.Pow(h / Range, 2)));
        }

        public double KrigingVariogramExponential(double h)
        {
            return Nugget + ((Sill - Nugget) / Range) * (1.0 - Math.Exp(-(1.0 / A) * (h / Range)));
        }

        public double KrigingVariogramSpherical(double h)
        {
            if (h > Range) return Nugget + (Sill - Nugget) / Range;
            else return Nugget + ((Sill - Nugget) / Range) * (1.5 * (h / Range) - 0.5 * Math.Pow(h / Range, 3));
        }

        public void Train(KrigingModel krigingModel, double sigma2, double alpha)
        {
            switch (krigingModel)
            {
                case KrigingModel.Gaussian:
                    {
                        Model = KrigingVariogramGaussian;
                        break;
                    }
                case KrigingModel.Exponential:
                    {
                        Model = KrigingVariogramExponential;
                        break;
                    }
                case KrigingModel.Spherical:
                    {
                        Model = KrigingVariogramSpherical;
                        break;
                    }
            }

            // Lag distance/semivariance
            int n = T.Length;
            int i, j, k, l;
            double[][] distance = new double[(n * n - n) / 2][];
            for (i = 0, k = 0; i < n; i++)
                for (j = 0; j < i; j++, k++)
                {
                    distance[k] = new double[2];
                    distance[k][0] = Math.Pow(Math.Pow(X[i] - X[j], 2) + Math.Pow(Y[i] - Y[j], 2), 0.5);
                    distance[k][1] = Math.Abs(T[i] - T[j]);
                }
            distance = distance.OrderBy(o => o[0]).ToArray();
            Range = distance[(n * n - n) / 2 - 1][0];

            // Bin lag distance
            var lags = ((n * n - n) / 2) > 30 ? 30 : (n * n - n) / 2;
            var tolerance = Range / lags;
            double[] lag = new double[lags], semi = new double[lags];
            if (lags < 30)
            {
                for (l = 0; l < lags; l++)
                {
                    lag[l] = distance[l][0];
                    semi[l] = distance[l][1];
                }
            }
            else
            {
                for (i = 0, j = 0, k = 0, l = 0; i < lags && j < ((n * n - n) / 2); i++, k = 0)
                {
                    while (distance[j][0] <= ((i + 1) * tolerance))
                    {
                        lag[l] += distance[j][0];
                        semi[l] += distance[j][1];
                        j++; k++;
                        if (j >= ((n * n - n) / 2)) break;
                    }
                    if (k > 0)
                    {
                        lag[l] /= k;
                        semi[l] /= k;
                        l++;
                    }
                }
                if (l < 2) return; // Error: Not enough points
            }

            // Feature transformation
            n = l;
            Range = lag[n - 1] - lag[0];
            double[] O = new double[2 * n], P = new double[n];
            for (i = 0; i < n; i++)
            {
                O[i * 2] = 1;
                switch (krigingModel)
                {
                    case KrigingModel.Gaussian:
                        {
                            O[i * 2 + 1] = 1.0 - Math.Exp(-(1.0 / A) * Math.Pow(lag[i] / Range, 2));
                            break;
                        }
                    case KrigingModel.Exponential:
                        {
                            O[i * 2 + 1] = 1.0 - Math.Exp(-(1.0 / A) * lag[i] / Range);
                            break;
                        }
                    case KrigingModel.Spherical:
                        {
                            O[i * 2 + 1] = 1.5 * (lag[i] / Range) - 0.5 * Math.Pow(lag[i] / Range, 3);
                            break;
                        }
                }
                P[i] = semi[i];
            }

            // Least squares
            double[] Ot = MatrixHelper.Transpose(O, n, 2);
            double[] Z = MatrixHelper.Multiply(Ot, O, 2, n, 2);
            Z = MatrixHelper.Add(Z, MatrixHelper.Diag(1 / alpha, 2));
            var cloneZ = MatrixHelper.Clone(Z);
            if (MatrixHelper.Chol(Z, 2))
                MatrixHelper.Chol2Inv(Z, 2);
            else
            {
                MatrixHelper.Solve(cloneZ, 2);
                Z = cloneZ;
            }
            double[] W = MatrixHelper.Multiply(MatrixHelper.Multiply(Z, Ot, 2, 2, n), P, 2, n, 1);

            // Variogram parameters
            Nugget = W[0];
            Sill = W[1] * Range + Nugget;

            // Gram matrix with prior
            n = T.Length;
            K = new double[n * n];
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < i; j++)
                {
                    K[i * n + j] = Model(Math.Pow(Math.Pow(X[i] - X[j], 2) + Math.Pow(Y[i] - Y[j], 2), 0.5));
                    K[j * n + i] = K[i * n + j];
                }
                K[i * n + i] = Model(0);
            }

            // Inverse penalized Gram matrix projected to target vector
            var C = MatrixHelper.Add(K, MatrixHelper.Diag(sigma2, n));
            var cloneC = MatrixHelper.Clone(C);
            if (MatrixHelper.Chol(C, n))
                MatrixHelper.Chol2Inv(C, n);
            else
            {
                MatrixHelper.Solve(cloneC, n);
                C = cloneC;
            }

            // Copy unprojected inverted matrix as K
            K = C;
            M = MatrixHelper.Multiply(C, T, n, n, 1);
        }

        public double Predict(double x, double y)
        {
            int n = T.Length;
            double[] k = new double[n];
            int i;
            double xi, yi;
            for (i = 0; i < n; i++)
            {
                xi = x - X[i];
                yi = y - Y[i];
                k[i] = Model(Math.Sqrt(xi * xi + yi * yi));
            }
            return MatrixHelper.Multiply(k, M, 1, n, 1)[0];
        }

        public double Variance(double x, double y)
        {
            int n = T.Length;
            double[] k = new double[n];
            int i;
            double xi, yi;
            for (i = 0; i < n; i++)
            {
                xi = x - X[i];
                yi = y - Y[i];
                k[i] = Model(Math.Sqrt(xi * xi + yi * yi));
            }
            return Model(0) + MatrixHelper.Multiply(MatrixHelper.Multiply(k, K, 1, n, n), k, 1, n, 1)[0];
        }
    }

    public enum KrigingModel
    {
        Gaussian,
        Exponential,
        Spherical
    }
}
