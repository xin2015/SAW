using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAW.WebApplication.Controllers
{
    public class OpenLayersController : Controller
    {
        // GET: OpenLayers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GeoJsonChina()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult TurfTest()
        {
            return View();
        }
    }
}