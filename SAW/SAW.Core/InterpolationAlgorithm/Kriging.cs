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
        public double[] TA { get; set; }
        public double[] XA { get; set; }
        public double[] YA { get; set; }
        public double Nugget { get; set; }
        public double Range { get; set; }
        public double Sill { get; set; }
        public double A { get; set; }
        public int N { get; set; }
        public Func<double, double> Model { get; set; }
        public double[][] K { get; set; }
        public double[][] M { get; set; }

        public Kriging()
        {
            A = 1 / 3;
        }

        public Kriging(double[] t, double[] x, double[] y) : this()
        {
            TA = t;
            XA = x;
            YA = y;
        }

        double[][] KrigingMatrixDiag(double c, int n)
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

        double[][] KrigingMatrixTranspose(double[][] X, int n, int m)
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

        double[][] KrigingMatrixAdd(double[][] X, double[][] Y, int n, int m)
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

        double[][] KrigingMatrixMultiply(double[][] X, double[][] Y, int n, int m, int p)
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

        bool KrigingMatrixChol(double[][] X, int n)
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

        void KrigingMatrixChol2inv(double[][] X, int n)
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

        bool KrigingMatrixSolve(double[][] X, int n)
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

        double KrigingVariogramGaussian(double h)
        {
            return Nugget + ((Sill - Nugget) / Range) * (1 - Math.Exp(-(1 / A) * Math.Pow(h / Range, 2)));
        }

        double KrigingVariogramExponential(double h)
        {
            return Nugget + ((Sill - Nugget) / Range) * (1 - Math.Exp(-(1 / A) * (h / Range)));
        }

        double KrigingVariogramSpherical(double h)
        {
            if (h > Range)
            {
                return Nugget + (Sill - Nugget) / Range;
            }
            else
            {
                return Nugget + ((Sill - Nugget) / Range) * (1.5 * (h / Range) - Math.Pow(h / Range, 3) / 2);
            }
        }

        public void Train(KrigingModel model, double sigma2, double alpha)
        {
            switch (model)
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
            int n = TA.Length;
            double[][] distance = new double[(n * n - n) / 2][];
            int i, j, k;
            for (i = 0, k = 0; i < n; i++)
            {
                for (j = i + 1; j < n; j++, k++)
                {
                    distance[k] = new double[2];
                    distance[k][0] = Math.Sqrt(Math.Pow(XA[i] - XA[j], 2) + Math.Pow(YA[i] - YA[j], 2));
                    distance[k][1] = Math.Abs(TA[i] - TA[j]);
                }
            }
            distance = distance.OrderBy(o => o[0]).ToArray();
            Range = distance.Last()[0];

            int lags = distance.Length > 30 ? 30 : distance.Length;
            double tolerance = Range / lags;
            double[] lag = new double[lags];
            double[] semi = new double[lags];
            int m = 0, count;
            if (lags < 30)
            {
                for (; m < lags; m++)
                {
                    lag[m] = distance[m][0];
                    semi[m] = distance[m][1];
                }
            }
            else
            {
                for (i = 0, j = 0; i < lags && j < distance.Length; i++)
                {
                    lag[i] = 0;
                    semi[i] = 0;
                    count = 0;
                    while (distance[j][0] <= (i + 1) * tolerance)
                    {
                        lag[m] += distance[j][0];
                        semi[m] += distance[j][1];
                        j++;
                        count++;
                        if (j == distance.Length)
                        {
                            break;
                        }
                    }
                    if (count > 0)
                    {
                        lag[m] /= count;
                        semi[m] /= count;
                        m++;
                    }
                }
            }
            if (m < 2)
            {
                throw new Exception("Error: Not enough points");
            }

            Range = lag.Last() - lag[0];
            double[][] X = new double[m][];
            double[][] Y = new double[m][];
            for (i = 0; i < m; i++)
            {
                X[i] = new double[2];
                X[i][0] = 1;
                switch (model)
                {
                    case KrigingModel.Gaussian:
                        {
                            X[i][1] = 1 - Math.Exp(-(1 / A) * Math.Pow(lag[i] / Range, 2));
                            break;
                        }
                    case KrigingModel.Exponential:
                        {
                            X[i][1] = 1 - Math.Exp(-(1 / A) * lag[i] / Range);
                            break;
                        }
                    case KrigingModel.Spherical:
                        {
                            X[i][1] = 1.5 * (lag[i] / Range) - 0.5 * Math.Pow(lag[i] / Range, 3);
                            break;
                        }
                }
                Y[i] = new double[] { semi[i] };
            }

            double[][] Xt = KrigingMatrixTranspose(X, m, 2);
            double[][] Z = KrigingMatrixMultiply(Xt, X, 2, m, 2);
            Z = KrigingMatrixAdd(Z, KrigingMatrixDiag(1 / alpha, 2), 2, 2);
            double[][] cloneZ = new double[Z.Length][];
            for (i = 0; i < Z.Length; i++)
            {
                double[] z = new double[Z[i].Length];
                Z[i].CopyTo(z, 0);
                cloneZ[i] = z;
            }
            if (KrigingMatrixChol(Z, 2))
            {
                KrigingMatrixChol2inv(Z, 2);
            }
            else
            {
                KrigingMatrixSolve(cloneZ, 2);
                Z = cloneZ;
            }
            double[][] W = KrigingMatrixMultiply(KrigingMatrixMultiply(Z, Xt, 2, 2, m), Y, 2, m, 1);

            Nugget = W[0][0];
            Sill = W[1][0] * Range + Nugget;
            N = n;

            double[][] K = new double[n][];
            for (i = 0; i < n; i++)
            {
                K[i] = new double[n];
                for (j = 0; j < i; j++)
                {
                    K[i][j] = Model(Math.Sqrt(Math.Pow(XA[i] - XA[j], 2) + Math.Pow(YA[i] - YA[j], 2)));
                    K[j][i] = K[i][j];
                }
                K[i][i] = Model(0);
            }

            double[][] C = KrigingMatrixAdd(K, KrigingMatrixDiag(sigma2, n), n, n);
            double[][] cloneC = new double[n][];
            for (i = 0; i < n; i++)
            {
                double[] c = new double[C[i].Length];
                C[i].CopyTo(c, 0);
                cloneC[i] = c;
            }
            if (KrigingMatrixChol(C, n))
            {
                KrigingMatrixChol2inv(C, n);
            }
            else
            {
                KrigingMatrixSolve(cloneC, n);
                C = cloneC;
            }

            double[][] T = new double[n][];
            for (i = 0; i < n; i++)
            {
                T[i] = new double[] { TA[i] };
            }
            double[][] M = KrigingMatrixMultiply(C, T, n, n, 1);
            this.K = C;
            this.M = M;
        }

        public double Predict(double x, double y)
        {
            var k = new double[N];
            for (int i = 0; i < N; i++)
            {
                k[i] = Model(Math.Sqrt(Math.Pow(x - XA[i], 2) + Math.Pow(y - YA[i], 2)));
            }
            double[][] K = new double[][] { k };
            return KrigingMatrixMultiply(K, M, 1, N, 1)[0][0];
        }
    }

    public enum KrigingModel
    {
        Gaussian,
        Exponential,
        Spherical
    }
}
