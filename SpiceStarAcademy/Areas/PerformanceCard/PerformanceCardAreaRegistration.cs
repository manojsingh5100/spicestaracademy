using System.Web.Mvc;

namespace SpiceStarAcademy.Areas.PerformanceCard
{
    public class PerformanceCardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PerformanceCard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PerformanceCard_default",
                "PerformanceCard/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}