using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SV18T1021214.BusinessLayer;

namespace SV18T1021214.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("account")]
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
        public ActionResult Login(String email, String password)
        {

            if (string.IsNullOrWhiteSpace(email))
                ModelState.AddModelError("email", "Email không được bỏ trống");
            if (string.IsNullOrWhiteSpace(password))
                ModelState.AddModelError("password", "Mật khẩu không được bỏ trống");
            else if(password.Length < 6 || password.Length>35)
                ModelState.AddModelError("password", "Độ dài mật khẩu không hợp lệ");
            if (!ModelState.IsValid)
                return View();

            if (CommonDataService.Login(email, password) != 0)
            {
                _ = User.Identity.Name;
                ViewBag.Username = email;
                FormsAuthentication.SetAuthCookie(email, true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Email = email;
                ViewBag.Password = password;
                ViewBag.Massage = "Đăng nhập không thành công";
                return View();
            }


            /* //TODO: Thay doi code de kiem tra dung tai khoan
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
             }*/
        }
        public ActionResult ChangePassword(String password, String rePassword)
        {  
            if (string.IsNullOrWhiteSpace(password))
                ModelState.AddModelError("password", "Mật khẩu không được bỏ trống");
            else if (password.Length < 6 || password.Length > 35)
                ModelState.AddModelError("password", "Độ dài mật khẩu không hợp lệ");
            if (string.IsNullOrWhiteSpace(rePassword))
                ModelState.AddModelError("repassword", "Mật khẩu không được bỏ trống");
            else if (password.Equals(rePassword) == false )
                ModelState.AddModelError("repassword", "Mật khẩu khác nhau");

            if (!ModelState.IsValid)
            {
                ViewBag.Password = password;
                ViewBag.RePassword = rePassword;
                ViewBag.Massage = "Đổi mật khẩu thất bại";
                ViewBag.Success = 0;
                return View();
            }
            CommonDataService.ChangePassword(User.Identity.Name, rePassword);
            ViewBag.Massage = "Đổi mật khẩu thành công";
            ViewBag.Success = 1;
            return View();

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

       
    }
}