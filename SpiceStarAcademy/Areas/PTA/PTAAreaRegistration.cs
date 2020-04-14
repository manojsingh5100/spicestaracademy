using System.Web.Mvc;

namespace SpiceStarAcademy.Areas.PTA
{
    public class PTAAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PTA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PTA_default",
                "PTA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}