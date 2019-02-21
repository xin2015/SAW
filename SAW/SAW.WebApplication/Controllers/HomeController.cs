using Newtonsoft.Json;
using SAW.Core.Extensions;
using SAW.Core.Helpers;
using System;
using System.Collections.Generic;
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

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Test(string text)
        {
            string result = RSAHelper.Default.Decrypt(text.FromBase64String(), false).ToUTF8String();
            LoginInfo li = JsonConvert.DeserializeObject<LoginInfo>(result);
            li.Time = li.Time.ToLocalTime();
            return Json(result, JsonRequestBehavior.DenyGet);
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
    }

    class LoginInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime Time { get; set; }
    }
}