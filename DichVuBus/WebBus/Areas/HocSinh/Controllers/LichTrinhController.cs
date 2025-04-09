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
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["Error"] = "Không xác định được người dùng.";
                    return View(new List<UserLichTrinhViewModel>());
                }

                var user = _context.Users.Find(u => u.Id == userId).FirstOrDefault();
                if (user == null)
                {
                    TempData["Error"] = "Không tìm thấy người dùng với userId: " + userId;
                    return View(new List<UserLichTrinhViewModel>());
                }

                var hocSinh = _context.HocSinh.Find(h => h.userId == userId).FirstOrDefault();
                if (hocSinh == null)
                {
                    TempData["Error"] = "Bạn chưa tham gia tuyến đường nào với userId: " + userId;
                    return View(new List<UserLichTrinhViewModel>());
                }

                var tuyenDuong = _context.TuyenDuong.Find(t => t.Id == hocSinh.tuyenDuongId).FirstOrDefault();
                if (tuyenDuong == null)
                {
                    TempData["Error"] = "Không tìm thấy tuyến đường với tuyenDuongId: " + hocSinh.tuyenDuongId;
                    return View(new List<UserLichTrinhViewModel>());
                }

                // Debug giá trị tuyenDuong.Id
                Console.WriteLine("tuyenDuong.Id: " + tuyenDuong.Id);
                var lichTrinhList = _context.LichTrinh.Find(l => l.tuyenDuongId == tuyenDuong.Id).ToList();
                Console.WriteLine("lichTrinhList count: " + lichTrinhList.Count);
                if (!lichTrinhList.Any())
                {
                    TempData["Error"] = "Không có lịch trình nào cho tuyến đường với tuyenDuongId: " + tuyenDuong.Id;
                    return View(new List<UserLichTrinhViewModel>());
                }

                var viewModelList = lichTrinhList.Select(l => new UserLichTrinhViewModel
                {
                    UserId = user.Id,
                    Username = user.username,
                    Role = user.role,
                    HoTen = hocSinh.hoTen,
                    TenTuyenDuong = tuyenDuong.tenTuyen,
                    ThoiGian = l.thoiGian,
                    Ngay = l.ngay
                }).ToList();

                var totalRecords = viewModelList.Count;
                var pagedList = viewModelList
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
                TempData["Error"] = "Đã xảy ra lỗi: " + ex.Message;
                return View(new List<UserLichTrinhViewModel>());
            }
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HuyLichTrinh(string userId)
        {
            try
            {
                // Kiểm tra userId hợp lệ
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["Error"] = "Không xác định được người dùng.";
                    return RedirectToAction("LichTrinhCuaBan", new { userId });
                }

                // Kiểm tra xem học sinh có tồn tại không
                var hocSinh = _context.HocSinh.Find(h => h.userId == userId).FirstOrDefault();
                if (hocSinh == null)
                {
                    TempData["Error"] = "Bạn chưa tham gia tuyến đường nào.";
                    return RedirectToAction("LichTrinhCuaBan", new { userId });
                }

                // Xóa bản ghi học sinh khỏi collection HocSinh
                var result = _context.HocSinh.DeleteOne(h => h.userId == userId);
                if (result.DeletedCount == 0)
                {
                    TempData["Error"] = "Không thể hủy lịch trình. Vui lòng thử lại.";
                    return RedirectToAction("LichTrinhCuaBan", new { userId });
                }

                TempData["Success"] = "Đã hủy lịch trình thành công!";
                return RedirectToAction("LichTrinhCuaBan", new { userId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi hủy lịch trình: " + ex.Message;
                return RedirectToAction("LichTrinhCuaBan", new { userId });
            }
        }
    }
}