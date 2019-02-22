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

        public ActionResult OpenLayersRasterReprojection()
        {
            return View();
        }

        public ActionResult LayerClipping()
        {
            return View();
        }

        public ActionResult OpenLayersBMap()
        {
            return View();
        }
        #endregion

        #region BMap
        public ActionResult BMap()
        {
            return View();
        }

        public ActionResult BaiDu()
        {
            return View();
        }

        public ActionResult BaiDuTileLayer()
        {
            return View();
        }

        public ActionResult BaiDuTileLayerTianDiTu()
        {
            return View();
        }
        #endregion

        #region BingMaps
        public ActionResult BingMaps()
        {
            return View();
        }

        public ActionResult BingMapsXYZoom()
        {
            return View();
        }
        #endregion
    }
}