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
            using (DataCenterServiceClient client = new DataCenterServiceClient())
            {
                StationHourData[] data = client.GetStationHourDataListFromHistoryByTime("GDAEIB", "2019!@GD", DateTime.Today.AddDays(-1));
                data = data.Where(o => o.AQI != "—").ToArray();
                List<double> t = new List<double>(), x = new List<double>(), y = new List<double>();
                int i = 0;
                double max = 0, lonm = 0, latm = 0, w = 0.5, h = 0.5;
                for (double lon = 73.2; lon <= 135.4; lon += w)
                {
                    for (double lat = 17.8; lat <= 53.8; lat += w)
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
                VariogramSrc variogram = KrigingSrc.Train(t.ToArray(), x.ToArray(), y.ToArray(), KrigingModelSrc.Exponential, 0, 100);
                List<double> list = new List<double>();
                Bitmap bitmap = new Bitmap(1001, 1001);
                i = 0;
                w = (135.4 - 73.2) / 1000;
                h = (53.8 - 17.8) / 1000;
                for (double lon = 73.2; lon <= 135.4; lon += w)
                {
                    int j = 0;
                    for (double lat = 17.8; lat <= 53.8; lat += h)
                    {
                        int aqi = (int)Math.Round(KrigingSrc.Predict(lon, lat, variogram));
                        bitmap.SetPixel(i, j++, Color.FromArgb(aqi / 256, aqi % 256, 0));
                    }
                    i++;
                }
                bitmap.Save("D:\\" + DateTime.Today.AddDays(-1).ToString("yyyyMMdd") + ".png");
                bitmap.Dispose();
            }
            return View();
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
    }

    class LoginInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime Time { get; set; }
    }
}