using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.InterpolationAlgorithm
{
    public class Kriging
    {
        // Matrix algebra
        static double[] KrigingMatrixDiag(double c, int n)
        {
            double[] Z = new double[n * n];
            int i;
            for (i = 0; i < n; i++)
            {
                Z[i * n + i] = c;
            }
            return Z;
        }

        static double[] KrigingMatrixTranspose(double[] X, int n, int m)
        {
            double[] Z = new double[m * n];
            int i, j;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < m; j++)
                {
                    Z[j * n + i] = X[i * m + j];
                }
            }
            return Z;
        }

        static void KrigingMatrixScale(double[] X, double c, int n, int m)
        {
            int i, j;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < m; j++)
                {
                    X[i * m + j] *= c;
                }
            }
        }

        static double[] KrigingMatrixAdd(double[] X, double[] Y, int n, int m)
        {
            double[] Z = new double[n * m];
            int i, j;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < m; j++)
                {
                    Z[i * m + j] = X[i * m + j] + Y[i * m + j];
                }
            }
            return Z;
        }

        // Naive matrix multiplication
        static double[] KrigingMatrixMultiply(double[] X, double[] Y, int n, int m, int p)
        {
            double[] Z = new double[n * p];
            int i, j, k;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < p; j++)
                {
                    Z[i * p + j] = 0;
                    for (k = 0; k < m; k++)
                    {
                        Z[i * p + j] += X[i * m + k] * Y[k * p + j];
                    }
                }
            }
            return Z;
        }

        // Cholesky decomposition
        static bool KrigingMatrixChol(double[] X, int n)
        {
            double[] p = new double[n];
            int i, j, k;
            for (i = 0; i < n; i++)
            {
                p[i] = X[i * n + i];
            }
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < i; j++)
                {
                    p[i] -= X[i * n + j] * X[i * n + j];
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
                        X[j * n + i] -= X[j * n + k] * X[i * n + k];
                    }
                    X[j * n + i] /= p[i];
                }
            }
            for (i = 0; i < n; i++)
            {
                X[i * n + i] = p[i];
            }
            return true;
        }

        // Inversion of cholesky decomposition
        static void KrigingMatrixChol2inv(double[] X, int n)
        {
            double sum;
            int i, j, k;
            for (i = 0; i < n; i++)
            {
                X[i * n + i] = 1 / X[i * n + i];
                for (j = i + 1; j < n; j++)
                {
                    sum = 0;
                    for (k = i; k < j; k++)
                    {
                        sum -= X[j * n + k] * X[k * n + i];
                    }
                    X[j * n + i] = sum / X[j * n + j];
                }
            }
            for (i = 0; i < n; i++)
            {
                for (j = i + 1; j < n; j++)
                {
                    X[i * n + j] = 0;
                }
            }
            for (i = 0; i < n; i++)
            {
                X[i * n + i] *= X[i * n + i];
                for (k = i + 1; k < n; k++)
                {
                    X[i * n + i] += X[k * n + i] * X[k * n + i];
                }
                for (j = i + 1; j < n; j++)
                {
                    for (k = j; k < n; k++)
                    {
                        X[i * n + j] += X[k * n + i] * X[k * n + j];
                    }
                }
            }
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < i; j++)
                {
                    X[i * n + j] = X[j * n + i];
                }
            }
        }

        // Inversion via gauss-jordan elimination
        static bool KrigingMatrixSolve(double[] X, int n)
        {
            int m = n;
            double[] b = new double[n * n];
            int[] indxc = new int[n];
            int[] indxr = new int[n];
            double[] ipiv = new double[n];
            int i, icol = 0, irow = 0, j, k, l, ll;
            double big, dum, pivinv, temp;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        b[i * n + j] = 1;
                    }
                    else
                    {
                        b[i * n + j] = 0;
                    }
                }
            }
            for (j = 0; j < n; j++)
            {
                ipiv[j] = 0;
            }
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
                                if (Math.Abs(X[j * n + k]) >= big)
                                {
                                    big = Math.Abs(X[j * n + k]);
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
                    for (l = 0; l < n; l++)
                    {
                        temp = X[irow * n + l];
                        X[irow * n + l] = X[icol * n + l];
                        X[icol * n + l] = temp;
                    }
                    for (l = 0; l < m; l++)
                    {
                        temp = b[irow * n + l];
                        b[irow * n + l] = b[icol * n + l];
                        b[icol * n + l] = temp;
                    }
                }
                indxr[i] = irow;
                indxc[i] = icol;

                if (X[icol * n + icol] == 0)
                {
                    return false; // Singular
                }

                pivinv = 1 / X[icol * n + icol];
                X[icol * n + icol] = 1;
                for (l = 0; l < n; l++)
                {
                    X[icol * n + l] *= pivinv;
                }
                for (l = 0; l < m; l++)
                {
                    b[icol * n + l] *= pivinv;
                }

                for (ll = 0; ll < n; ll++)
                {
                    if (ll != icol)
                    {
                        dum = X[ll * n + icol];
                        X[ll * n + icol] = 0;
                        for (l = 0; l < n; l++)
                        {
                            X[ll * n + l] -= X[icol * n + l] * dum;
                        }
                        for (l = 0; l < m; l++)
                        {
                            b[ll * n + l] -= b[icol * n + l] * dum;
                        }
                    }
                }
            }
            for (l = (n - 1); l >= 0; l--)
            {
                if (indxr[l] != indxc[l])
                {
                    for (k = 0; k < n; k++)
                    {
                        temp = X[k * n + indxr[l]];
                        X[k * n + indxr[l]] = X[k * n + indxc[l]];
                        X[k * n + indxc[l]] = temp;
                    }
                }
            }
            return true;
        }

        // Variogram models
        static double KrigingVariogramGaussian(double h, Variogram variogram)
        {
            return variogram.Nugget + ((variogram.Sill - variogram.Nugget) / variogram.Range) * (1.0 - Math.Exp(-Math.Pow(h / variogram.Range, 2) / variogram.A));
        }

        static double KrigingVariogramExponential(double h, Variogram variogram)
        {
            return variogram.Nugget + ((variogram.Sill - variogram.Nugget) / variogram.Range) * (1.0 - Math.Exp(-h / variogram.Range / variogram.A));
        }

        static double KrigingVariogramSpherical(double h, Variogram variogram)
        {
            if (h > variogram.Range)
            {
                return variogram.Nugget + (variogram.Sill - variogram.Nugget) / variogram.Range;
            }
            else
            {
                return variogram.Nugget + ((variogram.Sill - variogram.Nugget) / variogram.Range) * (1.5 * h / variogram.Range - 0.5 * Math.Pow(h / variogram.Range, 3));
            }
        }

        // Train using gaussian processes with bayesian priors
        public static Variogram Train(double[] t, double[] x, double[] y, KrigingModel model, double sigma2, double alpha)
        {
            Variogram variogram = new Variogram(t, x, y);
            switch (model)
            {
                case KrigingModel.Gaussian:
                    {
                        variogram.Model = KrigingVariogramGaussian;
                        break;
                    }
                case KrigingModel.Exponential:
                    {
                        variogram.Model = KrigingVariogramExponential;
                        break;
                    }
                case KrigingModel.Spherical:
                    {
                        variogram.Model = KrigingVariogramSpherical;
                        break;
                    }
            }

            // Lag distance/semivariance
            int i, j, k, l, n = t.Length;
            double[][] distance = new double[(n * n - n) / 2][];
            for (i = 0, k = 0; i < n; i++)
            {
                for (j = 0; j < i; j++, k++)
                {
                    distance[k] = new double[2];
                    distance[k][0] = Math.Pow(Math.Pow(x[i] - x[j], 2) + Math.Pow(y[i] - y[j], 2), 0.5);
                    distance[k][1] = Math.Abs(t[i] - t[j]);
                }
            }
            distance = distance.OrderBy(o => o[0]).ToArray();
            variogram.Range = distance[distance.Length - 1][0];

            // Bin lag distance
            int lags = distance.Length > 30 ? 30 : distance.Length;
            double tolerance = variogram.Range / lags;
            double[] lag = new double[lags];
            double[] semi = new double[lags];
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
                for (i = 0, j = 0, k = 0, l = 0; i < lags && j < distance.Length; i++, k = 0)
                {
                    while (distance[j][0] <= ((i + 1) * tolerance))
                    {
                        lag[l] += distance[j][0];
                        semi[l] += distance[j][1];
                        j++; k++;
                        if (j == distance.Length)
                        {
                            break;
                        }
                    }
                    if (k > 0)
                    {
                        lag[l] /= k;
                        semi[l] /= k;
                        l++;
                    }
                }
                if (l < 2)
                {
                    return variogram; // Error: Not enough points
                }
            }

            // Feature transformation
            n = l;
            variogram.Range = lag[n - 1] - lag[0];
            double[] X = new double[2 * n];
            double[] Y = new double[n];
            double A = variogram.A;
            for (i = 0; i < n; i++)
            {
                X[i * 2] = 1;
                switch (model)
                {
                    case KrigingModel.Gaussian:
                        {
                            X[i * 2 + 1] = 1.0 - Math.Exp(-(1.0 / A) * Math.Pow(lag[i] / variogram.Range, 2));
                            break;
                        }
                    case KrigingModel.Exponential:
                        {
                            X[i * 2 + 1] = 1.0 - Math.Exp(-(1.0 / A) * lag[i] / variogram.Range);
                            break;
                        }
                    case KrigingModel.Spherical:
                        {
                            X[i * 2 + 1] = 1.5 * (lag[i] / variogram.Range) - 0.5 * Math.Pow(lag[i] / variogram.Range, 3);
                            break;
                        }
                }
                Y[i] = semi[i];
            }

            // Least squares
            double[] Xt = KrigingMatrixTranspose(X, n, 2);
            double[] Z = KrigingMatrixMultiply(Xt, X, 2, n, 2);
            Z = KrigingMatrixAdd(Z, KrigingMatrixDiag(1 / alpha, 2), 2, 2);
            double[] cloneZ = new double[Z.Length];
            Z.CopyTo(cloneZ, 0);
            if (KrigingMatrixChol(Z, 2))
            {
                KrigingMatrixChol2inv(Z, 2);
            }
            else
            {
                KrigingMatrixSolve(cloneZ, 2);
                Z = cloneZ;
            }
            double[] W = KrigingMatrixMultiply(KrigingMatrixMultiply(Z, Xt, 2, 2, n), Y, 2, n, 1);

            // Variogram parameters
            variogram.Nugget = W[0];
            variogram.Sill = W[1] * variogram.Range + variogram.Nugget;
            variogram.N = x.Length;

            // Gram matrix with prior
            n = x.Length;
            double[] K = new double[n * n];
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < i; j++)
                {
                    K[i * n + j] = variogram.Model(Math.Pow(Math.Pow(x[i] - x[j], 2) + Math.Pow(y[i] - y[j], 2), 0.5), variogram);
                    K[j * n + i] = K[i * n + j];
                }
                K[i * n + i] = variogram.Model(0, variogram);
            }

            // Inverse penalized Gram matrix projected to target vector
            double[] C = KrigingMatrixAdd(K, KrigingMatrixDiag(sigma2, n), n, n);
            double[] cloneC = new double[C.Length];
            C.CopyTo(cloneC, 0);
            if (KrigingMatrixChol(C, n))
            {
                KrigingMatrixChol2inv(C, n);
            }
            else
            {
                KrigingMatrixSolve(cloneC, n);
                C = cloneC;
            }

            // Copy unprojected inverted matrix as K
            K = new double[C.Length];
            C.CopyTo(K, 0);
            double[] M = KrigingMatrixMultiply(C, t, n, n, 1);
            variogram.K = K;
            variogram.M = M;

            return variogram;
        }

        // Model prediction
        public static double Predict(double x, double y, Variogram variogram)
        {
            double[] k = new double[variogram.N];
            int i;
            for (i = 0; i < variogram.N; i++)
            {
                k[i] = variogram.Model(Math.Pow(Math.Pow(x - variogram.X[i], 2) + Math.Pow(y - variogram.Y[i], 2), 0.5), variogram);
            }
            return KrigingMatrixMultiply(k, variogram.M, 1, variogram.N, 1)[0];
        }

        public static double Variance(double x, double y, Variogram variogram)
        {
            double[] k = new double[variogram.N];
            int i;
            for (i = 0; i < variogram.N; i++)
            {
                k[i] = variogram.Model(Math.Pow(Math.Pow(x - variogram.X[i], 2) + Math.Pow(y - variogram.Y[i], 2), 0.5), variogram);
            }
            return variogram.Model(0, variogram) + KrigingMatrixMultiply(KrigingMatrixMultiply(k, variogram.K, 1, variogram.N, variogram.N), k, 1, variogram.N, 1)[0];
        }

        // Gridded matrices or contour paths
        public static void Grid(double[][][] polygons, Variogram variogram, double width)
        {
            int i, j, k, n = polygons.Length;
            if (n == 0)
            {
                return;
            }

            // Boundaries of polygons space
            double[] xlim = new double[] { polygons[0][0][0], polygons[0][0][0] };
            double[] ylim = new double[] { polygons[0][0][1], polygons[0][0][1] };
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < polygons[i].Length; j++)
                {
                    if (polygons[i][j][0] < xlim[0])
                    {
                        xlim[0] = polygons[i][j][0];
                    }
                    if (polygons[i][j][0] > xlim[1])
                    {
                        xlim[1] = polygons[i][j][0];
                    }
                    if (polygons[i][j][1] < ylim[0])
                    {
                        ylim[0] = polygons[i][j][1];
                    }
                    if (polygons[i][j][1] > ylim[1])
                    {
                        ylim[1] = polygons[i][j][1];
                    }
                }
            }

            // Alloc for O(n^2) space
            int[] a = new int[2], b = new int[2];
            double[] lxlim = new double[2], lylim = new double[2];
            int x = (int)Math.Ceiling((xlim[1] - xlim[0]) / width);
            int y = (int)Math.Ceiling((ylim[1] - ylim[0]) / width);

            int[][] A = new int[x + 1][];
            for (i = 0; i <= x; i++)
            {
                A[i] = new int[y + 1];
            }
            for (i = 0; i < n; i++)
            {
                // Range for polygons[i]
                lxlim[0] = polygons[i][0][0];
                lxlim[1] = lxlim[0];
                lylim[0] = polygons[i][0][1];
                lylim[1] = lylim[0];
                for (j = 1; j < polygons[i].Length; j++)
                {
                    if (polygons[i][j][0] < lxlim[0])
                    {
                        lxlim[0] = polygons[i][j][0];
                    }
                    if (polygons[i][j][0] > lxlim[1])
                    {
                        lxlim[1] = polygons[i][j][0];
                    }
                    if (polygons[i][j][1] < lylim[0])
                    {
                        lylim[0] = polygons[i][j][1];
                    }
                    if (polygons[i][j][1] > lylim[1])
                    {
                        lylim[1] = polygons[i][j][1];
                    }
                }

                // Loop through polygon subspace

            }
        }
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
