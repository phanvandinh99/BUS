using System.Web.Mvc;
using System.Web.Routing;

namespace WebBus
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Admin",
               url: "Admin",
               defaults: new { area = "Admin", controller = "Home", action = "Index" },
               namespaces: new[] { "WebBus.Areas.Admin.Controllers" }
            ).DataTokens.Add("area", "Admin");

            routes.MapRoute(
               name: "HocSinh",
               url: "HocSinh",
               defaults: new { area = "HocSinh", controller = "Home", action = "Index" },
               namespaces: new[] { "WebBus.Areas.HocSinh.Controllers" }
            ).DataTokens.Add("area", "HocSinh");

            routes.MapRoute(
               name: "Auth",
               url: "Auth",
               defaults: new { area = "Auth", controller = "Account", action = "Login" },
               namespaces: new[] { "WebBus.Areas.Auth.Controllers" }
           ).DataTokens.Add("area", "Auth");
        }
    }
}
