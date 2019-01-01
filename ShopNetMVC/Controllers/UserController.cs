using AutoMapper;
using Models.Common;
using Models.Common.Encode;
using Models.DataAccess;
using Models.DataAccess.Dto;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public ActionResult Login()
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
        public ActionResult Login(UserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = UserDao.Instance.checkLogin(model);
                switch (result)
                {
                    case Constants.LoginState.UsernameAndPasswordNull:
                        ModelState.AddModelError("", "Tài khoản và mật khẩu không được rỗng");
                        break;

                    case Constants.LoginState.UsernameNull:
                        ModelState.AddModelError("", "Tên đăng nhập bị rỗng");
                        break;

                    case Constants.LoginState.PasswordNull:
                        ModelState.AddModelError("", "Mật khẩu không được rỗng");
                        break;

                    case Constants.LoginState.Failed:
                        ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu");
                        break;

                    case Constants.LoginState.Locked:
                        ModelState.AddModelError("", "Tài khoản đã bị khóa");
                        break;

                    default:
                        var u = UserDao.Instance.getByID(model.UserName);
                        var userSession = new UserSession(u.UserID, u.FullName, u.GrantID);
                        Session.Add(Constants.USER_SESSION, userSession);
                        ViewBag.UserSession = true;
                        return RedirectToAction("Index", "Home");
                }
                ViewBag.UserSession = false;
            }
            else
            {
                ViewBag.UserSession = false;
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }

            return View(model);
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

        public ActionResult LogOut()
        {
            Session[Constants.USER_SESSION] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}