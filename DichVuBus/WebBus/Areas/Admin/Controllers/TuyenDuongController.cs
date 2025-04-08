using MongoDB.Driver;
using MongoDB.Bson;
using System.Web.Mvc;
using WebBus.Models;
using System.Linq;
using System.Collections.Generic;
using static WebBus.Models.ViewModels;

namespace WebBus.Areas.Admin.Controllers
{
    public class TuyenDuongController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        #region Hiển thị danh sách tuyến đường (với phân trang)
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var totalRecords = _context.TuyenDuong.Find(_ => true).CountDocuments();
            var tuyenDuongList = _context.TuyenDuong
                .Find(_ => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToList();

            // Join với XeBus để lấy BienSo
            var xeBusList = _context.XeBus.Find(_ => true).ToList();
            var tuyenDuongViewModel = tuyenDuongList.Select(t => new TuyenDuongViewModel
            {
                Id = t.Id,
                tenTuyen = t.tenTuyen,
                xeBusId = t.xeBusId,
                bienSoXe = string.IsNullOrEmpty(t.xeBusId) ? "Không xác định"
                    : xeBusList.FirstOrDefault(x => x.Id == t.xeBusId)?.bienSo ?? "Không xác định"
            }).ToList();

            ViewBag.TotalRecords = totalRecords;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)System.Math.Ceiling((double)totalRecords / pageSize);

            return View(tuyenDuongViewModel);
        }
        #endregion

        #region Thêm mới tuyến đường
        public ActionResult Create()
        {
            ViewBag.XeBusList = _context.XeBus.Find(_ => true).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(TuyenDuong tuyenDuong)
        {
            if (ModelState.IsValid)
            {
                _context.TuyenDuong.InsertOne(tuyenDuong);
                return RedirectToAction("Index");
            }
            ViewBag.XeBusList = _context.XeBus.Find(_ => true).ToList();
            return View(tuyenDuong);
        }
        #endregion

        #region Cập nhật tuyến đường
        public ActionResult Edit(string id)
        {
            var tuyenDuong = _context.TuyenDuong.Find(t => t.Id == id).FirstOrDefault();
            if (tuyenDuong == null) return HttpNotFound();
            ViewBag.XeBusList = _context.XeBus.Find(_ => true).ToList();
            return View(tuyenDuong);
        }

        [HttpPost]
        public ActionResult Edit(TuyenDuong tuyenDuong)
        {
            if (ModelState.IsValid)
            {
                _context.TuyenDuong.ReplaceOne(t => t.Id == tuyenDuong.Id, tuyenDuong);
                return RedirectToAction("Index");
            }
            ViewBag.XeBusList = _context.XeBus.Find(_ => true).ToList();
            return View(tuyenDuong);
        }
        #endregion

        #region Xem chi tiết tuyến đường
        public ActionResult Details(string id)
        {
            var tuyenDuong = _context.TuyenDuong.Find(t => t.Id == id).FirstOrDefault();
            if (tuyenDuong == null) return HttpNotFound();

            var xeBus = _context.XeBus
                .Find(x => x.Id == tuyenDuong.xeBusId)
                .FirstOrDefault();

            ViewBag.BienSoXe = xeBus?.bienSo ?? "Không xác định";
            return View(tuyenDuong);
        }
        #endregion

        #region Xóa tuyến đường
        public ActionResult Delete(string id)
        {
            var tuyenDuong = _context.TuyenDuong.Find(t => t.Id == id).FirstOrDefault();
            if (tuyenDuong == null) return HttpNotFound();
            return View(tuyenDuong);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            _context.TuyenDuong.DeleteOne(t => t.Id == id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}