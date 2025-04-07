using MongoDB.Driver;
using System.Linq;
using System.Web.Mvc;
using WebBus.Models;

namespace WebBus.Areas.Admin.Controllers
{
    public class HocSinhController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        #region Hiển thị danh sách học sinh
        public ActionResult Index()
        {
            var hocSinh = _context.HocSinh.Find(_ => true).ToList();
            return View(hocSinh);
        }
        #endregion

        #region Thêm mới học sinh

        #endregion

        #region Cập nhật học sinh

        #endregion

        #region Xem chi tiết học sinh

        #endregion

        #region Xóa học sinh

        #endregion
    }
}