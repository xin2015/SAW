using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.InterpolationAlgorithm
{
    public class Kriging
    {
        static double[][] KrigingMatrixDiag(double c, int n)
        {
            double[][] Z = new double[n][];
            double[] z;
            int i, j;
            for (i = 0; i < n; i++)
            {
                z = new double[n];
                for (j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        z[j] = c;
                    }
                }
                Z[i] = z;
            }
            return Z;
        }

        static double[][] KrigingMatrixTranspose(double[][] X, int n, int m)
        {
            double[][] Z = new double[m][];
            double[] z;
            int i, j;
            for (i = 0; i < m; i++)
            {
                z = new double[n];
                for (j = 0; j < n; j++)
                {
                    z[j] = X[j][i];
                }
                Z[i] = z;
            }
            return Z;
        }

        static double[][] KrigingMatrixAdd(double[][] X, double[][] Y, int n, int m)
        {
            double[][] Z = new double[n][];
            double[] x, y, z;
            int i, j;
            for (i = 0; i < n; i++)
            {
                x = X[i];
                y = Y[i];
                z = new double[m];
                for (j = 0; j < m; j++)
                {
                    z[j] = x[j] + y[j];
                }
                Z[i] = z;
            }
            return Z;
        }

        static double[][] KrigingMatrixMultiply(double[][] X, double[][] Y, int n, int m, int p)
        {
            double[][] Z = new double[n][];
            double[] z;
            int i, j, k;
            for (i = 0; i < n; i++)
            {
                z = new double[p];
                for (j = 0; j < p; j++)
                {
                    for (k = 0; k < m; k++)
                    {
                        z[j] += X[i][k] * Y[k][j];
                    }
                }
                Z[i] = z;
            }
            return Z;
        }

        static bool KrigingMatrixChol(double[][] X, int n)
        {
            double[] p = new double[n];
            int i, j, k;
            for (i = 0; i < n; i++)
            {
                p[i] = X[i][i];
            }
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < i; j++)
                {
                    p[i] -= Math.Pow(X[i][j], 2);
                }
                if (p[i] <= 0)
                {
                    return false;
                }
                p[i] = Math.Sqrt(p[i]);
                for (j = i + 1; j < n; j++)
                {
                    for (k = 0; k < i; k++)
                    {
                        X[j][i] -= X[j][k] * X[i][k];
                    }
                    X[j][i] /= p[i];
                }
            }
            for (i = 0; i < n; i++)
            {
                X[i][i] = p[i];
            }
            return true;
        }

        static void KrigingMatrixChol2inv(double[][] X, int n)
        {
            double sum;
            int i, j, k;
            for (i = 0; i < n; i++)
            {
                X[i][i] = 1 / X[i][i];
                for (j = i + 1; j < n; j++)
                {
                    sum = 0;
                    for (k = i; k < j; k++)
                    {
                        sum -= X[j][k] * X[k][i];
                    }
                    X[j][i] = sum / X[j][j];
                }
            }
            for (i = 0; i < n; i++)
            {
                for (j = i + 1; j < n; j++)
                {
                    X[i][j] = 0;
                }
            }
            for (i = 0; i < n; i++)
            {
                X[i][i] *= X[i][i];
                for (j = i + 1; j < n; j++)
                {
                    X[i][i] += Math.Pow(X[j][i], 2);
                }
                for (j = i + 1; j < n; j++)
                {
                    for (k = j; k < n; k++)
                    {
                        X[i][j] += X[k][i] * X[k][j];
                    }
                }
            }
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < i; j++)
                {
                    X[i][j] = X[j][i];
                }
            }
        }

        static bool KrigingMatrixSolve(double[][] X, int n)
        {
            double[][] Y = KrigingMatrixDiag(1, n);
            int[] indxc = new int[n], indxr = new int[n], ipiv = new int[n];
            int i, j, k, irow = 0, icol = 0;
            double big, temp, pivinv, dum;
            for (i = 0; i < n; i++)
            {
                big = 0;
                for (j = 0; j < n; j++)
                {
                    if (ipiv[j] != 1)
                    {
                        for (k = 0; k < n; k++)
                        {
                            if (ipiv[k] == 0)
                            {
                                if (Math.Abs(X[j][k]) >= big)
                                {
                                    big = Math.Abs(X[j][k]);
                                    irow = j;
                                    icol = k;
                                }
                            }
                        }
                    }
                }
                ++(ipiv[icol]);
                if (irow != icol)
                {
                    for (j = 0; j < n; j++)
                    {
                        temp = X[irow][j];
                        X[irow][j] = X[icol][j];
                        X[icol][j] = temp;
                        temp = Y[irow][j];
                        Y[irow][j] = Y[icol][j];
                        Y[icol][j] = temp;
                    }
                }
                indxr[i] = irow;
                indxc[i] = icol;
                if (X[icol][icol] == 0)
                {
                    return false;
                }
                pivinv = 1 / X[icol][icol];
                X[icol][icol] = 1;
                for (j = 0; j < n; j++)
                {
                    X[icol][j] *= pivinv;
                    Y[icol][j] *= pivinv;
                }
                for (j = 0; j < n; j++)
                {
                    if (j != icol)
                    {
                        dum = X[j][icol];
                        X[j][icol] = 0;
                        for (k = 0; k < n; k++)
                        {
                            X[j][k] -= X[icol][k] * dum;
                            Y[j][k] -= Y[icol][k] * dum;
                        }
                    }
                }
            }
            for (i = n - 1; i >= 0; i--)
            {
                if (indxr[i] != indxc[i])
                {
                    for (j = 0; j < n; j++)
                    {
                        temp = X[j][indxr[i]];
                        X[j][indxr[i]] = X[j][indxc[i]];
                        X[j][indxc[i]] = temp;
                    }
                }
            }
            return true;
        }

        static double KrigingVariogramGaussian(double h, double nugget, double range, double sill, double A)
        {
            return nugget + ((sill - nugget) / range) * (1 - Math.Exp(-(1 / A) * Math.Pow(h / range, 2)));
        }

        static double KrigingVariogramExponential(double h, double nugget, double range, double sill, double A)
        {
            return nugget + ((sill - nugget) / range) * (1 - Math.Exp(-(1 / A) * (h / range)));
        }

        static double KrigingVariogramSpherical(double h, double nugget, double range, double sill, double A)
        {
            if (h > range)
            {
                return nugget + (sill - nugget) / range;
            }
            else
            {
                return nugget + ((sill - nugget) / range) * (1.5 * (h / range) - Math.Pow(h / range, 3) / 2);
            }
        }

        //public static Variogram Train(double[] t, double[] x, double[] y, KrigingModel model, double sigma2, double alpha)
        //{
        //    Variogram variogram = new Variogram(t, x, y);

        //}
    }

    public class Variogram
    {
        public double[] T { get; set; }
        public double[] X { get; set; }
        public double[] Y { get; set; }
        public double Nugget { get; set; }
        public double Range { get; set; }
        public double Sill { get; set; }
        public double A { get; set; }
        public int N { get; set; }
        public Func<double, Variogram, double> Model { get; set; }
        public double[] K { get; set; }
        public double[] M { get; set; }

        public Variogram()
        {
            A = 1.0 / 3;
        }

        public Variogram(double[] t, double[] x, double[] y) : this()
        {
            T = t;
            X = x;
            Y = y;
        }
    }

    public enum KrigingModel
    {
        Gaussian,
        Exponential,
        Spherical
    }
}
