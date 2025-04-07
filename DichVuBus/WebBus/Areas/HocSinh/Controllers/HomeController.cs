using MongoDB.Bson;
using MongoDB.Driver;
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
            // Lấy danh sách tuyến đường
            var tuyenDuongList = _context.TuyenDuong.Find(_ => true).ToList();
            var xeBusList = _context.XeBus.Find(_ => true).ToList();

            // Tạo ViewModel cho TuyenDuong chứa cả tenTuyen và bienSoXe
            var tuyenDuongViewModel = tuyenDuongList.Select(t => new TuyenDuongViewModel
            {
                tenTuyen = t.tenTuyen,
                bienSoXe = string.IsNullOrEmpty(t.xeBusId) || !ObjectId.TryParse(t.xeBusId, out ObjectId xeBusId)
                    ? "Không xác định"
                    : xeBusList.FirstOrDefault(x => x.Id == xeBusId)?.bienSo ?? "Không xác định"
            }).ToList();

            ViewBag.TuyenDuongList = tuyenDuongViewModel;
            ViewBag.XeBusList = xeBusList;
            ViewBag.THPT = 10;
            ViewBag.THCS = 5;
            ViewBag.HocSinh = tuyenDuongList.Count;

            return View();
        }
    }
}