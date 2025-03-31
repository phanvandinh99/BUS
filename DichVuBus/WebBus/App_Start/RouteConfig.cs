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
               name: "Auth",
               url: "Auth",
               defaults: new { area = "Auth", controller = "Account", action = "Login" },
               namespaces: new[] { "WebBus.Areas.Auth.Controllers" }
           ).DataTokens.Add("area", "Auth");
        }
    }
}
