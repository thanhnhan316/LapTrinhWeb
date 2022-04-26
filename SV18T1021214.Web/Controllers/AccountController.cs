using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace SV18T1021214.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>

    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
        /// get: account
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(String username, String password)
        {
            //TODO: Thay doi code de kiem tra dung tai khoan
            if(username == "thanhnhantn05122000@gmail.com" && password == "1")
            {
                // ghi cookie ghi nhaanj phien dang nhap
                FormsAuthentication.SetAuthCookie(username, false);
                //quay lai trang chu
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Username = username;
                ViewBag.Message = "Dang nhap that bai";
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

       
    }
}