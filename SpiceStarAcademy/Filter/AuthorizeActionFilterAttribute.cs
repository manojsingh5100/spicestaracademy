using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;

namespace SpiceStarAcademy.Filter
{
    public class AuthorizeActionFilterAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined
                        (typeof(AllowAnonymousAttribute), true))
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                var userSignedIn = session["UserId"] as string;
                if (session["UserId"] == null || userSignedIn == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "" } });
                }
            }
        }

        public class LoggingPerFormanceFilterAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                {
                    HttpSessionStateBase session = filterContext.HttpContext.Session;
                    string controllerName = filterContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();
                    var userSignedIn = session["UserId"];
                    if (session["UserId"] == null || userSignedIn == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "" },{ "area","PerformanceCard" } });
                    }
                }
            }
        }


        public class LoggingFilterAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                {
                    HttpSessionStateBase session = filterContext.HttpContext.Session;
                    string controllerName = filterContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();
                    var userSignedIn = session["UserId"];
                    if (session["UserId"] == null || userSignedIn == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "" } });
                    }
                    else
                    {
                        if(Convert.ToString(session["RoleName"]) == "TeleCaller" && controllerName != "CallCenterInfo")
                        {
                            filterContext.Result = new RedirectResult("/Templates/ErrorPage.html");
                        }
                    }
                }

            }
        }

        public class CustomExceptionFilter : FilterAttribute,IExceptionFilter
        {
            public void OnException(ExceptionContext filterContext)
            {
                if (!filterContext.ExceptionHandled && filterContext.Exception is Exception)
                {
                    filterContext.Result = new RedirectResult("/Templates/ErrorPage.html");
                   // filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "" } });
                    filterContext.ExceptionHandled = true;
                }
            }
        }

    }
}