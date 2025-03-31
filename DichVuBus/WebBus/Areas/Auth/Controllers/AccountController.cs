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
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = _context.Users.Find(u => u.username == username && u.password == password).FirstOrDefault();
            if (user?.role == "Admin")
            {
                Session["UserId"] = user.Id.ToString();
                Session["Role"] = user.role;
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Sai thông tin đăng nhập!";
            return View();
        }
    }
}