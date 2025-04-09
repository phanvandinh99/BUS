using MongoDB.Driver;
using System;
using System.Linq;
using System.Web.Mvc;
using WebBus.Models;
using static WebBus.Models.ViewModels;

namespace WebBus.Areas.HocSinh.Controllers
{
    public class HomeController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        public ActionResult Index()
        {
            // Lấy danh sách tuyến đường và xe buýt để hiển thị
            var tuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList()
                    .Select(t => new TuyenDuongViewModel
                    {
                        Id = t.Id,
                        tenTuyen = t.tenTuyen,
                        xeBusId = t.xeBusId,
                        bienSoXe = _context.XeBus.Find(x => x.Id == t.xeBusId).FirstOrDefault()?.bienSo ?? "Chưa có xe",
                        LichTrinhs = _context.LichTrinh.Find(l => l.tuyenDuongId == t.Id).ToList()
                            .Select(l => new LichTrinhViewModel
                            {
                                Id = l.Id,
                                tuyenDuongId = l.tuyenDuongId,
                                thoiGian = l.thoiGian,
                                ngay = l.ngay
                            }).ToList()
                    }).ToList();

            ViewBag.TuyenDuongList = tuyenDuongList;

            var xeBusList = _context.XeBus.Find(_ => true).ToList();
            ViewBag.XeBusList = xeBusList;

            // Giả lập số liệu thống kê (có thể thay bằng truy vấn thực tế)
            ViewBag.THPT = 10; // Số trường THPT
            ViewBag.THCS = 5;  // Số trường THCS
            ViewBag.HocSinh = 20; // Tổng số xe buýt (có thể sửa lại)

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThamGiaTuyenDuong(string tuyenDuongId)
        {
            try
            {
                if (Session["HocSinh"] == null)
                {
                    TempData["Error"] = "Vui lòng đăng nhập để tham gia tuyến đường.";
                    return RedirectToAction("Index");
                }

                var user = (User)Session["HocSinh"];
                if (string.IsNullOrEmpty(tuyenDuongId))
                {
                    TempData["Error"] = "Vui lòng chọn tuyến đường.";
                    return RedirectToAction("Index");
                }

                // Kiểm tra xem tuyến đường có tồn tại không
                var tuyenDuong = _context.TuyenDuong.Find(t => t.Id == tuyenDuongId).FirstOrDefault();
                if (tuyenDuong == null)
                {
                    TempData["Error"] = "Tuyến đường không tồn tại.";
                    return RedirectToAction("Index");
                }

                // Kiểm tra xem học sinh đã tham gia tuyến đường NÀY chưa
                var existingHocSinh = _context.HocSinh.Find(h => h.userId == user.Id && h.tuyenDuongId == tuyenDuongId).FirstOrDefault();
                if (existingHocSinh != null)
                {
                    TempData["Error"] = "Bạn đã tham gia tuyến đường này rồi.";
                    return RedirectToAction("Index");
                }

                // Thêm thông tin học sinh vào collection HocSinh
                var hocSinh = new WebBus.Models.HocSinh
                {
                    hoTen = user.username, // Có thể yêu cầu nhập họ tên thực tế
                    lop = "Chưa xác định", // Có thể thêm form để nhập lớp
                    tuyenDuongId = tuyenDuongId,
                    userId = user.Id
                };
                _context.HocSinh.InsertOne(hocSinh);

                TempData["Success"] = "Đã tham gia tuyến đường thành công!";
                return RedirectToAction("LichTrinhCuaBan", "LichTrinh", new { userId = user.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi tham gia tuyến đường: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}