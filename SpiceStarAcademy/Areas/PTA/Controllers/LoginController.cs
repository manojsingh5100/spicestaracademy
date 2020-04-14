using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJModel;
using SJService.PTA;

namespace SpiceStarAcademy.Areas.PTA.Controllers
{
    public class LoginController : Controller
    {
        LoginService loginService = null;
        public LoginController()
        {
            loginService = new LoginService();
        }
        // GET: PTA/Login
        public ActionResult Index()
        {
            Session.RemoveAll();
            LoginViewModel loginInfo = new LoginViewModel();
            if (Request.Cookies["Login"] != null)
            {
                loginInfo.Email = Request.Cookies["Login"].Values["EmailID"];
                loginInfo.Password = Request.Cookies["Login"].Values["Password"];
                loginInfo.RememberMe = true;
            }
            return View(loginInfo);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel Model)
        {
            LoginViewModel loginInfo = new LoginViewModel();
            Model.IsPerformanceDept = true;
            try
            {
                loginInfo = loginService.GetLoginInfo(Model);
                if (loginInfo != null)
                {
                    Session["UserName"] = loginInfo.Fname + " " + loginInfo.LName;
                    Session["Designation"] = loginInfo.Designation;
                    Session["Department"] = loginInfo.Department;
                    Session["UserId"] = loginInfo.Id;
                    Session["RoleName"] = loginInfo.RoleName;
                    if (Model.RememberMe)
                    {
                        HttpCookie cookie = new HttpCookie("Login");
                        cookie.Values.Add("EmailID", loginInfo.Email);
                        cookie.Values.Add("Password", loginInfo.Password);
                        cookie.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        var c = new HttpCookie("Login");
                        c.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(c);
                    }
                    return RedirectToAction("", "DashBoard",new { area="PTA" });
                }
                else
                    TempData["errorMsg"] = "Invalid username and password!";
                if (loginInfo == null)
                    loginInfo = Model;
                return View(loginInfo);
            }
            catch (Exception ex)
            {
                loginInfo.errorMessage = ex.Message + "//" + ex.InnerException;
                TempData["errorMsg"] = ex.Message + "//" + ex.InnerException;
                return View(loginInfo);
            }
        }
    }
}