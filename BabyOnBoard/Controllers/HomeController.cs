using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BabyOnBoard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Baby on board";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Ovidiu Gliga";

            return View();
        }
    }
}