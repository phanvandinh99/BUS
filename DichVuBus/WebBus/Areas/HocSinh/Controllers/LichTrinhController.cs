using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBus.Models;
using static WebBus.Models.ViewModels;

namespace WebBus.Areas.HocSinh.Controllers
{
    public class LichTrinhController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        #region Lịch Trình Của Bạn
        public ActionResult LichTrinhCuaBan(string userId, int page = 1, int pageSize = 5)
        {
            try
            {
                // Sử dụng aggregation pipeline
                var pipeline = _context.Users.Aggregate()
                    .Match(u => u.Id == userId)
                    .Lookup("HocSinh", "Id", "userId", "HocSinh") // Join với HocSinh
                    .Unwind("HocSinh") // Mở rộng mảng HocSinh
                    .Lookup("TuyenDuong", "HocSinh.tuyenDuongId", "_id", "TuyenDuong") // Join với TuyenDuong
                    .Unwind("TuyenDuong") // Mở rộng mảng TuyenDuong
                    .Lookup("LichTrinh", "TuyenDuong._id", "tuyenDuongId", "LichTrinh") // Join với LichTrinh
                    .Unwind("LichTrinh") // Mở rộng mảng LichTrinh
                    .Project<UserLichTrinhViewModel>(Builders<BsonDocument>.Projection
                        .Include("Id")           // UserId
                        .Include("username")     // Username
                        .Include("role")         // Role
                        .Include("HocSinh.hoTen") // HoTen
                        .Include("TuyenDuong.tenTuyen") // TenTuyenDuong
                        .Include("LichTrinh.thoiGian")  // ThoiGian
                        .Include("LichTrinh.ngay"));    // Ngay

                // Debug: Kiểm tra kết quả pipeline
                var result = pipeline.ToList();
                if (result.Count == 0)
                {
                    TempData["Error"] = "Không tìm thấy dữ liệu lịch trình nào.";
                    return View(new List<UserLichTrinhViewModel>());
                }

                // Lấy tổng số bản ghi
                var totalRecords = result.Count;

                // Phân trang
                var pagedList = result
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.TotalRecords = totalRecords;
                ViewBag.PageSize = pageSize;
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                return View(pagedList);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi tải danh sách người dùng đăng ký lịch trình: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        #endregion

        // GET: HocSinh/LichTrinh
        public ActionResult Index()
        {
            return View();
        }
    }
}