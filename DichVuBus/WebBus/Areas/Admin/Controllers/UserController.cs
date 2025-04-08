using MongoDB.Driver;
using System.Linq;
using System.Web.Mvc;
using WebBus.Models;

namespace WebBus.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        #region Hiển thị danh sách user
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var totalRecords = _context.Users.Find(_ => true).CountDocuments();
            var userList = _context.Users
                .Find(_ => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToList();

            ViewBag.TotalRecords = totalRecords;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)System.Math.Ceiling((double)totalRecords / pageSize);

            return View(userList);
        }
        #endregion

        #region Thêm mới users
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(WebBus.Models.User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.InsertOne(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }
        #endregion

        #region Cập nhật users
        public ActionResult Edit(string id)
        {
            var user = _context.Users.Find(x => x.Id == id).FirstOrDefault();
            if (user == null) return HttpNotFound();
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(WebBus.Models.User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.ReplaceOne(x => x.Id == user.Id, user);
                return RedirectToAction("Index");
            }
            return View(user);
        }
        #endregion

        #region Xem chi tiết users
        public ActionResult Details(string id)
        {
            var user = _context.Users.Find(x => x.Id == id).FirstOrDefault();
            if (user == null) return HttpNotFound();
            return View(user);
        }
        #endregion

        #region Xóa users
        public ActionResult Delete(string id)
        {
            var user = _context.Users.Find(x => x.Id == id).FirstOrDefault();
            if (user == null) return HttpNotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            _context.Users.DeleteOne(x => x.Id == id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}