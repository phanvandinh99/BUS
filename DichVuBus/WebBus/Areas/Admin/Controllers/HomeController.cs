using System.Web.Mvc;
using WebBus.Models;

namespace WebBus.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly MongoDBContext _context = new MongoDBContext();

        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}