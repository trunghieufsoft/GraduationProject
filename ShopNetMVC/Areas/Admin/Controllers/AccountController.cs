using AutoMapper;
using Models.Common;
using Models.DataAccess;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShopNetMVC.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        #region Action Result

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            SetGrantViewBag();
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserRequestDto user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = Mapper.Map<User>(user);
                    UserDao.Instance.insert(model);
                }
                catch
                {
                    ModelState.AddModelError("", "Thêm tài khoản thất bại!");
                    SetGrantViewBag();
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var user = UserDao.Instance.getByID(id);
            var model = Mapper.Map<UserRequestDto>(user);
            SetGrantViewBag(model.GrantID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserRequestDto user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserDao.Instance.update(user);
                }
                catch
                {
                    ModelState.AddModelError("", "Sửa tài khoản thất bại!");
                    SetGrantViewBag(user.GrantID);
                    return View();
                }
            }

            return RedirectToAction("Index");
        }

        private void SetGrantViewBag(int? selectedID = null)
        {
            var grants = GrantDao.Instance.getObjectList();
            ViewBag.GrandID = new SelectList(grants, "GrantID", "GrantName", selectedID);
        }

        #endregion Action Result

        #region Json Result

        public JsonResult ChangeStatus(string id)
        {
            var result = UserDao.Instance.changeStatus(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string id)
        {
            var result = UserDao.Instance.delete(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadData(int page, int pageSize, string search)
        {
            var users = string.IsNullOrEmpty(search) ? UserDao.Instance.getObjectList("") : UserDao.Instance.getObjectList(search);
            var data = Mapper.Map<List<UserRequestDto>>(users);
            var response = data.Skip((page - 1) * pageSize).Take(pageSize);
            foreach (var item in response)
                item.GrantName = GrantDao.Instance.getByID(UserDao.Instance.getByID(item.UserID).GrantID).GrantName;
            return Json(new
            {
                data = response,
                total = data.Count,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Json Result
    }
}