using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAW.WebApplication.Controllers
{
    public class MapController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TianDiTu()
        {
            return View();
        }

        #region OpenLayers
        public ActionResult OpenLayers()
        {
            return View();
        }

        public ActionResult OpenLayersIndex()
        {
            return View();
        }

        public ActionResult OpenLayersBingMaps()
        {
            return View();
        }
        #endregion

        public ActionResult BaiDu()
        {
            return View();
        }
    }
}