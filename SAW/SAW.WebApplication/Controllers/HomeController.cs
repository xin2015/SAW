using Newtonsoft.Json;
using SAW.Core.Extensions;
using SAW.Core.Helpers;
using SAW.Core.InterpolationAlgorithm;
using SAW.WebApplication.DataCenterService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAW.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetGuangZhouAirQualityPNG()
        {
            List<double> t = new List<double>(), x = new List<double>(), y = new List<double>();
            double[] extent = new double[] { 112.94, 22.49, 114.1, 23.97 };
            string fileName;
            using (DataCenterServiceClient client = new DataCenterServiceClient())
            {
                StationHourData[] data = client.GetStationHourDataListFromHistoryByTime("GDAEIB", "2019!@GD", new DateTime(2020, 6, 23, 17, 0, 0));
                data = data.Where(o => o.AQI != "—").ToArray();
                double[] gdExtent = new double[] { 109.46, 20.05, 117.48, 25.63 };
                foreach (StationHourData item in data)
                {
                    double xi = double.Parse(item.Longitude), yi = double.Parse(item.Latitude);
                    if (xi >= gdExtent[0] && xi <= gdExtent[2] && yi >= gdExtent[1] && yi <= gdExtent[3])
                    {
                        double ti = double.Parse(item.AQI);
                        t.Add(ti);
                        x.Add(xi);
                        y.Add(yi);
                    }
                }
                fileName = string.Format("D:\\440100_{0}.png", data.First().TimePoint.ToString("yyyyMMddHH"));
            }
            double resolution = (extent[2] - extent[0]) / 1023;
            BitmapHelper.DrawGridByKriging(t.ToArray(), x.ToArray(), y.ToArray(), extent, resolution, fileName);
            return Content("Done");
        }

        public ActionResult GetGuangDongAirQualityPNG()
        {
            List<double> t = new List<double>(), x = new List<double>(), y = new List<double>();
            double[] extent = new double[] { 109.46, 20.05, 117.48, 25.63 };
            string fileName;
            using (DataCenterServiceClient client = new DataCenterServiceClient())
            {
                StationHourData[] data = client.GetStationHourDataListFromHistoryByTime("GDAEIB", "2019!@GD", new DateTime(2020, 6, 23, 17, 0, 0));
                data = data.Where(o => o.AQI != "—").ToArray();
                double[] tExtent = new double[] { 106.79, 18.19, 120.15, 27.49 };
                foreach (StationHourData item in data)
                {
                    double xi = double.Parse(item.Longitude), yi = double.Parse(item.Latitude);
                    if (xi >= tExtent[0] && xi <= tExtent[2] && yi >= tExtent[1] && yi <= tExtent[3])
                    {
                        double ti = double.Parse(item.AQI);
                        t.Add(ti);
                        x.Add(xi);
                        y.Add(yi);
                    }
                }
                fileName = string.Format("D:\\440000_{0}.png", data.First().TimePoint.ToString("yyyyMMddHH"));
            }
            double resolution = (extent[2] - extent[0]) / 1023;
            BitmapHelper.DrawGridByKriging(t.ToArray(), x.ToArray(), y.ToArray(), extent, resolution, fileName);
            return Content("Done");
        }

        public ActionResult GetChinaAirQualityPNG()
        {
            List<double> t = new List<double>(), x = new List<double>(), y = new List<double>();
            double[] extent = new double[] { 73.2, 17.8, 135.4, 53.8 };
            string fileName1;
            string fileName2;
            string fileName3;
            string fileName4;
            using (DataCenterServiceClient client = new DataCenterServiceClient())
            {
                StationHourData[] data = client.GetStationHourDataListFromLive("GDAEIB", "2019!@GD");
                data = data.Where(o => o.AQI != "—").ToArray();
                int i = 0;
                double max = 0, lonm = 0, latm = 0, w = 0.5, h = 0.5;
                for (double lon = extent[0]; lon <= extent[2]; lon += w)
                {
                    for (double lat = extent[1]; lat <= extent[3]; lat += h)
                    {
                        max = 0;
                        for (i = 0; i < data.Length; i++)
                        {
                            StationHourData item = data[i];
                            double xi = double.Parse(item.Longitude), yi = double.Parse(item.Latitude);
                            if (xi >= lon && xi <= lon + w && yi >= lat && yi <= lat + w)
                            {
                                double ti = double.Parse(item.AQI);
                                if (ti > max)
                                {
                                    max = ti;
                                    lonm = xi;
                                    latm = yi;
                                }
                            }
                        }
                        if (max > 0)
                        {
                            t.Add(max);
                            x.Add(lonm);
                            y.Add(latm);
                        }
                    }
                }
                fileName1 = string.Format("D:\\{0}_IDW1.png", data.First().TimePoint.ToString("yyyyMMddHH"));
                fileName2 = string.Format("D:\\{0}_IDW2.png", data.First().TimePoint.ToString("yyyyMMddHH"));
                fileName3 = string.Format("D:\\{0}_IDW3.png", data.First().TimePoint.ToString("yyyyMMddHH"));
                fileName4 = string.Format("D:\\{0}_IDW4.png", data.First().TimePoint.ToString("yyyyMMddHH"));
            }
            double resolution = (extent[2] - extent[0]) / 1023;
            BitmapHelper.DrawGridByIDW1(t.ToArray(), x.ToArray(), y.ToArray(), extent, resolution, fileName1);
            BitmapHelper.DrawGridByIDW2(t.ToArray(), x.ToArray(), y.ToArray(), extent, resolution, fileName2);
            BitmapHelper.DrawGridByIDW3(t.ToArray(), x.ToArray(), y.ToArray(), extent, resolution, fileName3);
            BitmapHelper.DrawGridByIDW4(t.ToArray(), x.ToArray(), y.ToArray(), extent, resolution, fileName4);
            return Content("Done");
        }

        public ActionResult Test()
        {
            ViewData["PublicKey"] = RSAHelper.Default.ExportJavaParameters(false);
            return View();
        }

        [HttpPost]
        public ActionResult Test(string text)
        {
            string result = RSAHelper.Default.Decrypt(text.FromBase64String(), false).ToUTF8String();
            LoginInfo li = JsonConvert.DeserializeObject<LoginInfo>(result);
            li.Time = li.Time.ToLocalTime();
            return Json(li, JsonRequestBehavior.DenyGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FileExists(string path)
        {
            path = Server.MapPath(path);
            return Json(System.IO.File.Exists(path), JsonRequestBehavior.AllowGet);
        }

        public FilePathResult FileDownload(string fileName)
        {
            return File(fileName, "application/octet-stream", System.IO.Path.GetFileName(fileName));
        }

        public ActionResult Uploadify()
        {
            return View();
        }

        public JsonResult Upload(FormCollection fc)
        {
            //ILog logger = LogManager.GetLogger("Upload");
            bool status;
            string message;
            try
            {
                HttpPostedFileBase file = Request.Files[0];
                string imgType = fc["imgType"];
                string filePath = "/Content/Upload/";
                string absolutePath = Server.MapPath(filePath);
                if (!Directory.Exists(absolutePath))//判断上传文件夹是否存在，若不存在，则创建
                {
                    Directory.CreateDirectory(absolutePath);//创建文件夹
                }
                file.SaveAs(absolutePath + file.FileName);
                status = true;
                message = "图片上传成功！";
            }
            catch (Exception e)
            {
                //logger.Error("上传图片失败！", e);
                status = false;
                message = "图片上传失败！";
            }
            return Json(new { Status = status, Message = message }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult GroundOverlay()
        {
            return View();
        }

        public ActionResult TestTDT()
        {
            return View();
        }

        public ActionResult GetBrowserInfo()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Platform", Request.Browser.Platform);
            dic.Add("Id", Request.Browser.Id);
            dic.Add("Browser", Request.Browser.Browser);
            dic.Add("Version", Request.Browser.Version);
            dic.Add("Type", Request.Browser.Type);
            dic.Add("IsMobileDevice", Request.Browser.IsMobileDevice.ToString());
            dic.Add("MobileDeviceModel", Request.Browser.MobileDeviceModel);
            dic.Add("Cookies", Request.Browser.Cookies.ToString());
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequestInfo()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Platform", Request.Browser.Platform);
            dic.Add("Id", Request.Browser.Id);
            dic.Add("Browser", Request.Browser.Browser);
            dic.Add("Version", Request.Browser.Version);
            dic.Add("Type", Request.Browser.Type);
            dic.Add("IsMobileDevice", Request.Browser.IsMobileDevice.ToString());
            dic.Add("MobileDeviceModel", Request.Browser.MobileDeviceModel);
            dic.Add("Cookies", Request.Browser.Cookies.ToString());
            dic.Add("HttpMethod", Request.HttpMethod);
            dic.Add("UserAgent", Request.UserAgent);
            dic.Add("UserHostName", Request.UserHostName);
            dic.Add("UserHostAddress", Request.UserHostAddress);
            dic.Add("Path", Request.Path);
            dic.Add("RawUrl", Request.RawUrl);
            dic.Add("AbsoluteUri", Request.Url.AbsoluteUri);
            dic.Add("AbsolutePath", Request.Url.AbsolutePath);
            dic.Add("Host", Request.Url.Host);
            dic.Add("Port", Request.Url.Port.ToString());
            dic.Add("PathAndQuery", Request.Url.PathAndQuery);
            dic.Add("LocalPath", Request.Url.LocalPath);
            dic.Add("Query", Request.Url.Query);
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Temp()
        {
            return View();
        }

        public ActionResult GetAQIData()
        {
            using (DataCenterServiceClient client = new DataCenterServiceClient())
            {
                StationHourData[] data = client.GetStationHourDataListFromHistoryByTime("GDAEIB", "2019!@GD", DateTime.Today.AddDays(-1));
                data = data.Where(o => o.AQI != "—").ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TianQi()
        {
            return View();
        }

        public ActionResult TestPNG()
        {
            return View();
        }

        public ActionResult TestPolygons()
        {
            return View();
        }

        public ActionResult GIS()
        {
            return View();
        }

        public ActionResult GISGuangDong()
        {
            return View();
        }

        public ActionResult GISGuangZhou()
        {
            return View();
        }

        public ActionResult DrawChinaPolygons()
        {
            return View();
        }

        public ActionResult DrawChinaPolygons2()
        {
            return View();
        }

        public ActionResult DrawGuangDongPolygons()
        {
            return View();
        }

        public ActionResult DrawGuangZhouPolygons()
        {
            return View();
        }

        public ActionResult TestIDW(DateTime time)
        {
            IDW idw;
            Kriging kriging;
            using (DataCenterServiceClient client = new DataCenterServiceClient())
            {
                StationHourData[] data = client.GetStationHourDataListFromHistoryByTime("GDAEIB", "2019!@GD", time);
                data = data.Where(o => o.AQI != "—").ToArray();
                double[] X = new double[data.Length], Y = new double[data.Length], T = new double[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    StationHourData item = data[i];
                    double x = double.Parse(item.Longitude), y = double.Parse(item.Latitude);
                    double t = double.Parse(item.AQI);
                    X[i] = x;
                    Y[i] = y;
                    T[i] = t;
                }
                idw = new IDW(X, Y, T, 2);
                kriging = new Kriging(X, Y, T);
            }
            double temp = idw.Predict(112, 36);
            kriging.Train(KrigingModel.Exponential, 0, 100);
            temp = kriging.Predict(112, 36);
            return Content(temp.ToString());
        }
    }

    class LoginInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime Time { get; set; }
    }
}