using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Web.Mvc;
using WebBus.Models;
using static WebBus.Models.ViewModels;

namespace WebBus.Areas.Admin.Controllers
{
    public class LichTrinhController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        #region Hiển thị danh sách lịch trình (với phân trang)
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var totalRecords = _context.LichTrinh.Find(_ => true).CountDocuments();
            var lichTrinhList = _context.LichTrinh
                .Find(_ => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToList();

            // Join với TuyenDuong để lấy tenTuyen
            var tuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();
            var lichTrinhViewModel = lichTrinhList.Select(l => new LichTrinhViewModel
            {
                Id = l.Id,
                tuyenDuongId = l.tuyenDuongId,
                tenTuyenDuong = string.IsNullOrEmpty(l.tuyenDuongId) ? "Không xác định"
                                                    : tuyenDuongList.FirstOrDefault(t => t.Id == l.tuyenDuongId)?.tenTuyen ?? "Không xác định",
                thoiGian = l.thoiGian,
                ngay = l.ngay
            }).ToList();

            ViewBag.TotalRecords = totalRecords;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)System.Math.Ceiling((double)totalRecords / pageSize);

            return View(lichTrinhViewModel);
        }
        #endregion

        #region Thêm mới lịch trình
        public ActionResult Create()
        {
            ViewBag.TuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(LichTrinh lichTrinh)
        {
            if (ModelState.IsValid)
            {
                _context.LichTrinh.InsertOne(lichTrinh);
                return RedirectToAction("Index");
            }
            ViewBag.TuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();
            return View(lichTrinh);
        }
        #endregion

        #region Cập nhật lịch trình
        public ActionResult Edit(string id)
        {
            var lichTrinh = _context.LichTrinh.Find(l => l.Id == id).FirstOrDefault();
            if (lichTrinh == null) return HttpNotFound();
            ViewBag.TuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();
            return View(lichTrinh);
        }

        [HttpPost]
        public ActionResult Edit(LichTrinh lichTrinh)
        {
            if (ModelState.IsValid)
            {
                _context.LichTrinh.ReplaceOne(l => l.Id == lichTrinh.Id, lichTrinh);
                return RedirectToAction("Index");
            }
            ViewBag.TuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();
            return View(lichTrinh);
        }
        #endregion

        #region Xem chi tiết lịch trình
        public ActionResult Details(string id)
        {
            var lichTrinh = _context.LichTrinh.Find(l => l.Id == id).FirstOrDefault();
            if (lichTrinh == null) return HttpNotFound();

            var tuyenDuong = _context.TuyenDuong
                .Find(t => t.Id == lichTrinh.tuyenDuongId)
                .FirstOrDefault();

            ViewBag.TenTuyenDuong = tuyenDuong?.tenTuyen ?? "Không xác định";
            return View(lichTrinh);
        }
        #endregion

        #region Xóa lịch trình
        public ActionResult Delete(string id)
        {
            var lichTrinh = _context.LichTrinh.Find(l => l.Id == id).FirstOrDefault();
            if (lichTrinh == null) return HttpNotFound();
            return View(lichTrinh);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            _context.LichTrinh.DeleteOne(l => l.Id == id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}