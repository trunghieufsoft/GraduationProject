using AutoMapper;
using Models.DataAccess;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShopNetMVC.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
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
        public ActionResult Create(CategoryRequestDto category)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<Category>(category);
                if (CategoryDao.Instance.insert(model))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm danh mục sản phẩm thất bại!");
                }
            }
            return View(category);
        }

        public ActionResult Edit(byte id)
        {
            var category = CategoryDao.Instance.getByID(id);
            var model = Mapper.Map<CategoryRequestDto>(category);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CategoryRequestDto category)
        {
            if (ModelState.IsValid)
            {
                if (CategoryDao.Instance.update(category))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Sửa danh mục sản phẩm thất bại!");
                }
            }
            return View();
        }

        #endregion Action Result

        #region Json Result

        public JsonResult loadData(int page, int pageSize, string search)
        {
            var categories = string.IsNullOrEmpty(search) ? CategoryDao.Instance.getObjectList() : CategoryDao.Instance.getObjectList(search);
            var data = Mapper.Map<List<Category>, List<CategoryRequestDto>>(categories.ToList());
            var response = data.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                data = response,
                total = data.Count,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(byte id)
        {
            var result = CategoryDao.Instance.changeStatus(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(byte id)
        {
            var result = CategoryDao.Instance.delete(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Json Result
    }
}