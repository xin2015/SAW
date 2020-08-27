using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    public static class MatrixHelper
    {
        public static double[] Diag(double c, int n)
        {
            double[] Z = new double[n * n];
            int i;
            for (i = 0; i < n; i++) Z[i * n + i] = c;
            return Z;
        }

        public static double[] Clone(double[] X)
        {
            int n = X.Length;
            double[] Z = new double[n];
            int i;
            for (i = 0; i < n; i++) Z[i] = X[i];
            return Z;
        }

        public static double[] Transpose(double[] X, int n, int m)
        {
            double[] Z = new double[m * n];
            int i, j;
            for (i = 0; i < n; i++)
                for (j = 0; j < m; j++)
                    Z[j * n + i] = X[i * m + j];
            return Z;
        }

        public static void Scale(double[] X, double c)
        {
            int n = X.Length;
            int i;
            for (i = 0; i < n; i++)
                X[i] *= c;
        }

        public static double[] Add(double[] X, double[] Y)
        {
            int n = X.Length;
            double[] Z = new double[n];
            int i;
            for (i = 0; i < n; i++)
                Z[i] = X[i] + Y[i];
            return Z;
        }

        public static double[] Multiply(double[] X, double[] Y, int n, int m, int p)
        {
            double[] Z = new double[n * p];
            int i, j, k;
            for (i = 0; i < n; i++)
                for (j = 0; j < p; j++)
                    for (k = 0; k < m; k++)
                        Z[i * p + j] += X[i * m + k] * Y[k * p + j];
            return Z;
        }

        public static bool Chol(double[] X, int n)
        {
            double[] p = new double[n];
            int i, j, k;
            for (i = 0; i < n; i++) p[i] = X[i * n + i];
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < i; j++)
                    p[i] -= X[i * n + j] * X[i * n + j];
                if (p[i] <= 0) return false;
                p[i] = Math.Sqrt(p[i]);
                for (j = i + 1; j < n; j++)
                {
                    for (k = 0; k < i; k++)
                        X[j * n + i] -= X[j * n + k] * X[i * n + k];
                    X[j * n + i] /= p[i];
                }
            }
            for (i = 0; i < n; i++) X[i * n + i] = p[i];
            return true;
        }

        public static void Chol2Inv(double[] X, int n)
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
                        sum -= X[j * n + k] * X[k * n + i];
                    X[j * n + i] = sum / X[j * n + j];
                }
            }
            for (i = 0; i < n; i++)
                for (j = i + 1; j < n; j++)
                    X[i * n + j] = 0;
            for (i = 0; i < n; i++)
            {
                X[i * n + i] *= X[i * n + i];
                for (k = i + 1; k < n; k++)
                    X[i * n + i] += X[k * n + i] * X[k * n + i];
                for (j = i + 1; j < n; j++)
                    for (k = j; k < n; k++)
                        X[i * n + j] += X[k * n + i] * X[k * n + j];
            }
            for (i = 0; i < n; i++)
                for (j = 0; j < i; j++)
                    X[i * n + j] = X[j * n + i];

        }

        public static bool Solve(double[] X, int n)
        {
            int m = n;
            double[] b = new double[n * n];
            int[] indxc = new int[n], indxr = new int[n], ipiv = new int[n];
            int icol = 0, irow = 0, i, j, k, l, ll;
            double big, dum, pivinv, temp;

            for (i = 0; i < n; i++)
                for (j = 0; j < n; j++)
                {
                    if (i == j) b[i * n + j] = 1;
                    else b[i * n + j] = 0;
                }
            for (j = 0; j < n; j++) ipiv[j] = 0;
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

                if (X[icol * n + icol] == 0) return false; // Singular

                pivinv = 1 / X[icol * n + icol];
                X[icol * n + icol] = 1;
                for (l = 0; l < n; l++) X[icol * n + l] *= pivinv;
                for (l = 0; l < m; l++) b[icol * n + l] *= pivinv;

                for (ll = 0; ll < n; ll++)
                {
                    if (ll != icol)
                    {
                        dum = X[ll * n + icol];
                        X[ll * n + icol] = 0;
                        for (l = 0; l < n; l++) X[ll * n + l] -= X[icol * n + l] * dum;
                        for (l = 0; l < m; l++) b[ll * n + l] -= b[icol * n + l] * dum;
                    }
                }
            }
            for (l = (n - 1); l >= 0; l--)
                if (indxr[l] != indxc[l])
                {
                    for (k = 0; k < n; k++)
                    {
                        temp = X[k * n + indxr[l]];
                        X[k * n + indxr[l]] = X[k * n + indxc[l]];
                        X[k * n + indxc[l]] = temp;
                    }
                }

            return true;
        }
    }
}
