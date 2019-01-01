using AutoMapper;
using Models.Common;
using Models.Common.Encode;
using Models.DataAccess;
using Models.DataAccess.Dto;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopNetMVC.Controllers
{
    public class UserController : BaseController
    {
        // GET: Register
        [HttpGet]
        public ActionResult Register()
        {
            //var related = ProductDao.Instance.RelatedProducts(4);
            var related = ProductDao.Instance.Recommendations();

            ViewBag.Related = Mapper.Map<List<Product>, List<ProductRequestDto>>(related.ToList());

            var listPrice = new List<string>();
            foreach (var item in related)
                listPrice.Add(Converter.formatPrice(item.Cost));
            ViewBag.listPrice = listPrice;
            ViewBag.Length = listPrice.Count;

            return View();
        }

        [HttpPost]
        public ActionResult Register(UserRegister model)
        {
            if (ModelState.IsValid)
            {
                var existed = UserDao.Instance.checkUserName(model.UserID);
                if (existed)
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                }
                else
                {
                    var user = new User();
                    user.UserID = model.UserID;
                    user.Password = model.Password;
                    user.FullName = model.FullName;
                    user.Address = model.Address;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.CreatedAt = DateTime.Now;
                    user.isActive = true;
                    user.GrantID = (int)Constants.GrantID.User;
                    var result = UserDao.Instance.insert(user);
                    if (result)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Địa chỉ email hoặc số điện thoại đã tồn tại");
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Login(string uname, string passwd)
        {
            UserDto model = new UserDto()
            {
                UserName = uname,
                Password = passwd,
                RememberMe = true
            };
            string msg = ""; Constants.LoginState result = Constants.LoginState.Failed;
            try
            {
                result = UserDao.Instance.checkLogin(model);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
            switch (result)
            {
                case Constants.LoginState.UsernameAndPasswordNull:
                    msg = "Tài khoản và mật khẩu không được rỗng";
                    break;

                case Constants.LoginState.UsernameNull:
                    msg = "Tên đăng nhập bị rỗng";
                    break;

                case Constants.LoginState.PasswordNull:
                    msg = "Mật khẩu không được rỗng";
                    break;

                case Constants.LoginState.Failed:
                    msg = "Tên đăng nhập hoặc mật khẩu không đúng";
                    break;

                case Constants.LoginState.Locked:
                    msg = "Tài khoản đã bị khóa";
                    break;

                case Constants.LoginState.Successed:
                default:
                    var use = UserDao.Instance.getByID(model.UserName);
                    var userSession = new UserSession(use.UserID, use.FullName, use.GrantID);
                    Session.Add(Constants.USER_SESSION, userSession);

                    return Json(new { result = true, message = "đăng nhập thành công" });
            }

            return Json(new { result = false, message = msg });
        }

        [HttpPost]
        public JsonResult GetUserInfo()
        {
            try
            {
                var session = (UserSession)Session[Constants.USER_SESSION];
                var user = UserDao.Instance.getByID(session.UserName);
                return Json(new { user });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult LogOut()
        {
            try
            {
                Session[Constants.USER_SESSION] = null;
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }
    }
}