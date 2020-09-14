using SAW.Core.InterpolationAlgorithm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    public static class BitmapHelper
    {
        public static void DrawGrid(double[] extent, double resolution, IPredict predict, string fileName)
        {
            int width = (int)Math.Round((extent[2] - extent[0]) / resolution) + 1, height = (int)Math.Round((extent[3] - extent[1]) / resolution) + 1;
            Bitmap bitmap = new Bitmap(width, height);
            double lon, lat;
            for (int i = 0; i < width; i++)
            {
                lon = extent[0] + i * resolution;
                for (int j = 0; j < height; j++)
                {
                    lat = extent[3] - j * resolution;
                    int value = (int)Math.Round(predict.Predict(lon, lat));
                    bitmap.SetPixel(i, j, Color.FromArgb(value / 256, value % 256, 0));
                }
            }
            bitmap.Save(fileName);
            bitmap.Dispose();
        }

        public static void DrawGridByKriging(double[] t, double[] x, double[] y, double[] extent, double resolution, string fileName)
        {
            Kriging kriging = new Kriging(x, y, t);
            kriging.Train(KrigingModel.Exponential, 0, 100);
            DrawGrid(extent, resolution, kriging, fileName);
        }

        public static void DrawGridByIDW(double[] t, double[] x, double[] y, double[] extent, double resolution, string fileName)
        {
            IDW predict = new IDW(x, y, t);
            DrawGrid(extent, resolution, predict, fileName);
        }
    }
}
