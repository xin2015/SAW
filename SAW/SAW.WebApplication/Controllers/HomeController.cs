using System;
using System.Collections.Generic;
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
    }
}