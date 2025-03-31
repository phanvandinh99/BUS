using MongoDB.Driver;
using System.Web.Mvc;
using WebBus.Models;

namespace WebBus.Areas.Auth.Controllers
{
    public class AccountController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        public ActionResult Login()
        {
            var hocSinh = _context.HocSinh.Find(_ => true).ToList();

            return View();
        }

    }
}