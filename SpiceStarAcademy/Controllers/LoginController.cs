using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJModel;
using SJService;
using SpiceStarAcademy.Helper;
using SpiceStarAcademy.Models;

namespace SpiceStarAcademy.Controllers
{
    public class LoginController : Controller
    {
        private LoginService loginService = null;
        public LoginController()
        {
            loginService = new LoginService();
        }
        // GET: Login
        public ActionResult Index()
        {
            var monthName = DateTime.Now.ToString("MMMM");
            //FeeManagementService ff = new FeeManagementService();
            //var data = ff.GetEmailNotificatioInfo(1000);
            //SpiceStarAcademy.Models.Common c = new SpiceStarAcademy.Models.Common();
            // c.SendFeeNotificationToCadidates();
            //c.SendFeeNotificationToCadidates();
            //========================================================
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
        public ActionResult Index(LoginViewModel model)
        {
            //NPFParameters dd = new NPFParameters
            //{
            //    application_no = "2019225",
            //    email = "foundoncemusic@gmail.com",
            //    application_stage = "",
            //    enrolled_campus = "Yes",
            //    enrolled_department = "Clear",
            //};
            //SpiceStarAcademy.Models.Common cc = new SpiceStarAcademy.Models.Common();
            //cc.SendCandidateStatusInfo(dd);

            //==================================================
            LoginViewModel loginInfo = new LoginViewModel();
            model.IsPerformanceDept = false;
            try
            {
                loginInfo = loginService.GetLoginInfo(model);
                if (loginInfo != null)
                {
                    Session["UserName"] = loginInfo.Fname + " " + loginInfo.LName;
                    Session["Designation"] = loginInfo.Designation;
                    Session["Department"] = loginInfo.Department;
                    Session["UserId"] = loginInfo.Id;
                    Session["RoleName"] = loginInfo.RoleName;
                    Session["CurrentYear"] = DateTime.Now.Year;
                    if (model.RememberMe)
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
                    if (loginInfo.RoleName == "TeleCaller")
                        return RedirectToAction("", "CallCenterInfo");
                    else
                        return RedirectToAction("", "DashBoard");
                }
                else
                    TempData["errorMsg"] = "Invalid username and password!";
                if (loginInfo == null)
                    loginInfo = model;
                return View(loginInfo);
            }
            catch (Exception ex)
            {
                loginInfo.errorMessage = ex.Message + "//" + ex.InnerException;
                TempData["errorMsg"] = ex.Message + "//" + ex.InnerException;
                return View(loginInfo);
            }
        }

        public ActionResult UserRegistration()
        {
            LoginViewModel model = new LoginViewModel();
            model.RoleList = loginService.GetRoleList();
            return View(model);
        }

        [HttpPost]
        public ActionResult UserRegistration(LoginViewModel model)
        {
            model.EnteredBy = Convert.ToInt32(Session["UserId"]);
            bool status = loginService.SaveUserRegistration(model);
            if(status)
                TempData["errorMsg"] = "Registration Succesfully Done!";
            return RedirectToAction("UserRegistration", "Login");
        }

        [HttpGet]
        public JsonResult IsExistEmail(string Email)
        {
            return Json(loginService.IsExistEmail(Email), JsonRequestBehavior.AllowGet);
        }

    }
}