using MongoDB.Driver;
using MongoDB.Bson;
using System.Web.Mvc;
using WebBus.Models;
using System.Linq;

namespace WebBus.Areas.Admin.Controllers
{
    public class XeBusController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        #region Hiển thị danh sách xe buýt
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var totalRecords = _context.XeBus.Find(_ => true).CountDocuments();
            var xeBusList = _context.XeBus
                .Find(_ => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToList();

            ViewBag.TotalRecords = totalRecords;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)System.Math.Ceiling((double)totalRecords / pageSize);

            return View(xeBusList);
        }
        #endregion

        #region Thêm mới xe buýt
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(XeBus xeBus)
        {
            if (ModelState.IsValid)
            {
                _context.XeBus.InsertOne(xeBus);
                return RedirectToAction("Index");
            }
            return View(xeBus);
        }
        #endregion

        #region Cập nhật xe buýt
        public ActionResult Edit(string id)
        {
            var xeBus = _context.XeBus.Find(x => x.Id == new ObjectId(id)).FirstOrDefault();
            if (xeBus == null) return HttpNotFound();
            return View(xeBus);
        }

        [HttpPost]
        public ActionResult Edit(XeBus xeBus)
        {
            if (ModelState.IsValid)
            {
                _context.XeBus.ReplaceOne(x => x.Id == xeBus.Id, xeBus);
                return RedirectToAction("Index");
            }
            return View(xeBus);
        }
        #endregion

        #region Xem chi tiết xe buýt
        public ActionResult Details(string id)
        {
            var xeBus = _context.XeBus.Find(x => x.Id == new ObjectId(id)).FirstOrDefault();
            if (xeBus == null) return HttpNotFound();
            return View(xeBus);
        }
        #endregion

        #region Xóa xe buýt
        public ActionResult Delete(string id)
        {
            var xeBus = _context.XeBus.Find(x => x.Id == new ObjectId(id)).FirstOrDefault();
            if (xeBus == null) return HttpNotFound();
            return View(xeBus);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            _context.XeBus.DeleteOne(x => x.Id == new ObjectId(id));
            return RedirectToAction("Index");
        }
        #endregion
    }
}