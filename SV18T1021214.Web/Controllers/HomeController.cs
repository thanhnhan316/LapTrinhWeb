﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV18T1021214.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>

    [Authorize]
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

  /*      public ActionResult Categories()
        {

            var model = SV18T1021214.BusinessLayer.CommonDataService.Category_List();
            return View(model);
        }*/
    }
}