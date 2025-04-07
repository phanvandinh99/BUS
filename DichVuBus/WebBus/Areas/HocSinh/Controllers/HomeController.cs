using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBus.Areas.HocSinh.Controllers
{
    public class HomeController : Controller
    {
        // GET: HocSinh/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}