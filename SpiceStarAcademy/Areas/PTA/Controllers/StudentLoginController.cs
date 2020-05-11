using SJModel;
using SJService.PTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpiceStarAcademy.Areas.PTA.Controllers
{
    public class StudentLoginController : Controller
    {

        StudentLoginService loginService = null;
        public StudentLoginController()
        {
            loginService = new StudentLoginService();
        }
        // GET: PTA/StudentLogin
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
                loginInfo = loginService.GetStudentLogin(Model);
                if (loginInfo != null)
                {
                    Session["UserName"] = loginInfo.Fname + " " + loginInfo.LName;
                    Session["Designation"] = loginInfo.Designation;
                    Session["Department"] = loginInfo.Department;
                    Session["UserId"] = loginInfo.Id;
                    Session["RoleName"] = loginInfo.RoleName;
                    Session["ApplicationNo"] = loginInfo.ApplicationNo;
                    Session["Admissionid"] = loginInfo.Admissionid;
                    Session["ptaRegistrationInfoId"] = loginInfo.ptaRegistrationInfoId;
                    Session["ptaPilotRegistrationMasterId"] = loginInfo.ptaPilotRegistrationMasterId;
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

                    return RedirectToAction("", "StudentProfile", new { area = "PTA" });
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