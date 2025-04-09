using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Web.Mvc;
using WebBus.Models;
using static WebBus.Models.ViewModels;

namespace WebBus.Areas.Admin.Controllers
{
    public class HocSinhController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        #region Hiển thị danh sách học sinh (với phân trang)
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var totalRecords = _context.HocSinh.Find(_ => true).CountDocuments();
            var hocSinhList = _context.HocSinh
                .Find(_ => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToList();

            // Join với TuyenDuong để lấy TenTuyen
            var tuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();
            var hocSinhViewModel = hocSinhList.Select(h => new HocSinhViewModel
            {
                Id = h.Id,
                userId = h.userId,
                hoTen = h.hoTen,
                lop = h.lop,
                tuyenDuongId = h.tuyenDuongId,
                tenTuyenDuong = string.IsNullOrEmpty(h.tuyenDuongId) ? "Không xác định"
                                : tuyenDuongList.FirstOrDefault(t => t.Id == h.tuyenDuongId)?.tenTuyen ?? "Không xác định"
            }).ToList();

            ViewBag.TotalRecords = totalRecords;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)System.Math.Ceiling((double)totalRecords / pageSize);

            return View(hocSinhViewModel);
        }
        #endregion

        #region Thêm mới học sinh
        public ActionResult Create()
        {
            // Load danh sách tuyến đường
            ViewBag.TuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();

            // Lấy danh sách tất cả userId đã được sử dụng trong HocSinh
            var usedUserIds = _context.HocSinh.Find(_ => true).ToList().Select(h => h.userId).ToList();

            // Lấy danh sách Users có role "HocSinh" và chưa được sử dụng
            ViewBag.UserList = _context.Users.Find(u => u.role == "HocSinh" && !usedUserIds.Contains(u.Id)).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WebBus.Models.HocSinh hocSinh)
        {
            if (ModelState.IsValid)
            {
                _context.HocSinh.InsertOne(hocSinh);
                return RedirectToAction("Index");
            }
            // Load lại danh sách nếu form không hợp lệ
            ViewBag.TuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();
            var usedUserIds = _context.HocSinh.Find(_ => true).ToList().Select(h => h.userId).ToList();
            ViewBag.UserList = _context.Users.Find(u => u.role == "HocSinh" && !usedUserIds.Contains(u.Id)).ToList();
            return View(hocSinh);
        }
        #endregion

        #region Cập nhật học sinh
        public ActionResult Edit(string id)
        {
            var hocSinh = _context.HocSinh.Find(h => h.Id == id).FirstOrDefault();
            if (hocSinh == null) return HttpNotFound();

            // Load danh sách tuyến đường
            ViewBag.TuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();

            // Lấy danh sách tất cả userId đã được sử dụng trong HocSinh, trừ học sinh hiện tại
            var usedUserIds = _context.HocSinh.Find(h => h.Id != id).ToList().Select(h => h.userId).ToList();

            // Lấy danh sách Users có role "HocSinh" và (chưa được sử dụng hoặc là userId hiện tại)
            ViewBag.UserList = _context.Users.Find(u => u.role == "HocSinh" && (!usedUserIds.Contains(u.Id) || u.Id == hocSinh.userId)).ToList();

            return View(hocSinh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WebBus.Models.HocSinh hocSinh)
        {
            if (ModelState.IsValid)
            {
                _context.HocSinh.ReplaceOne(h => h.Id == hocSinh.Id, hocSinh);
                return RedirectToAction("Index");
            }
            // Load lại danh sách nếu form không hợp lệ
            ViewBag.TuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();
            var usedUserIds = _context.HocSinh.Find(h => h.Id != hocSinh.Id).ToList().Select(h => h.userId).ToList();
            ViewBag.UserList = _context.Users.Find(u => u.role == "HocSinh" && (!usedUserIds.Contains(u.Id) || u.Id == hocSinh.userId)).ToList();
            return View(hocSinh);
        }
        #endregion

        #region Xem chi tiết học sinh (hiển thị tên tuyến đường)
        public ActionResult Details(string id)
        {
            var hocSinh = _context.HocSinh.Find(h => h.Id == id).FirstOrDefault();
            if (hocSinh == null) return HttpNotFound();

            var tuyenDuong = _context.TuyenDuong
                .Find(t => t.Id == hocSinh.tuyenDuongId)
                .FirstOrDefault();

            ViewBag.TenTuyenDuong = tuyenDuong?.tenTuyen ?? "Không xác định";
            return View(hocSinh);
        }
        #endregion

        #region Xóa học sinh
        public ActionResult Delete(string id)
        {
            var hocSinh = _context.HocSinh.Find(h => h.Id == id).FirstOrDefault();
            if (hocSinh == null) return HttpNotFound();
            return View(hocSinh);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            _context.HocSinh.DeleteOne(h => h.Id == id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}