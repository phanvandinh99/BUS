using MongoDB.Driver;
using System.Drawing;
using System.Web.Mvc;
using WebBus.Models;

namespace WebBus.Areas.Auth.Controllers
{
    public class AccountController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        #region Đăng Nhập
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
                Session["Admin"] = user;
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else if (user?.role == "HocSinh")
            {
                Session["HocSinh"] = user;
                return RedirectToAction("Index", "Home", new { area = "HocSinh" });
            }

            ViewBag.Error = "Sai thông tin đăng nhập!";
            return View();
        }

        #endregion

        #region Đăng Xuất

        public ActionResult DangXuatAdmin()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login", "Account");
        }

        public ActionResult DangXuatHocSinh()
        {
            Session["HocSinh"] = null;
            return RedirectToAction("Login", "Account");
        }

        #endregion


    }
}