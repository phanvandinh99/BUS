using System.Web.Mvc;

namespace WebBus.Areas.HocSinh
{
    public class HocSinhAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HocSinh";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HocSinh_default",
                "HocSinh/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}