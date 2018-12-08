using AutoMapper;
using Models.DataAccess;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShopNetMVC.Areas.Admin.Controllers
{
    public class GrantController : BaseController
    {
        #region Action Result

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(GrantRequestDto grant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = Mapper.Map<Grant>(grant);
                    GrantDao.Instance.insert(model);
                }
                catch
                {
                    ModelState.AddModelError("", "Thêm phân quyền thất bại!");
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(byte id)
        {
            var grant = GrantDao.Instance.getByID(id);
            var model = Mapper.Map<GrantRequestDto>(grant);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(GrantRequestDto grantDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GrantDao.Instance.changeGrantName(grantDto);
                }
                catch
                {
                    ModelState.AddModelError("", "Chỉnh sửa phân quyền thất bại!");
                    return View(grantDto);
                }
            }

            return RedirectToAction("Index");
        }

        #endregion Action Result

        #region Json Result

        [HttpGet]
        public JsonResult loadData(int page, int pageSize, string search)
        {
            var grants = string.IsNullOrEmpty(search) ? GrantDao.Instance.getObjectList() : GrantDao.Instance.getObjectList(search);

            var data = Mapper.Map<List<GrantRequestDto>>(grants);

            var response = data.Skip((page - 1) * pageSize).Take(pageSize);

            return Json(new
            {
                data = response,
                total = data.Count,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult changeStatus(byte id)
        {
            try
            {
                GrantDao.Instance.changeStatus(id);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                respone = true
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Json Result
    }
}