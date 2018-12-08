using AutoMapper;
using Models.DataAccess;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShopNetMVC.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        #region ActionResult

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            var prod = ProductDao.Instance.getByID(id);
            var model = Mapper.Map<ProductRequestDto>(prod);
            return View(model);
        }

        public ActionResult Create()
        {
            SetCategoryViewBag();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ProductRequestDto model)
        {
            if (ModelState.IsValid)
            {
                var product = Mapper.Map<Product>(model);
                if (ProductDao.Instance.insert(product))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sản phẩm thất bại!");
                }
            }
            SetCategoryViewBag();
            return View();
        }

        public ActionResult Edit(int id)
        {
            var product = ProductDao.Instance.getByID(id);
            var model = Mapper.Map<ProductRequestDto>(product);
            SetCategoryViewBag(model.CateID);
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(ProductRequestDto model)
        {
            if (ModelState.IsValid)
            {
                if (ProductDao.Instance.UpdateExceptImage(model))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Sửa sản phẩm thất bại!");
                }
            }
            SetCategoryViewBag(model.CateID);
            return View(model);
        }

        public void SetCategoryViewBag(int? selectedID = null)
        {
            var listCategory = CategoryDao.Instance.GetListCategory();
            ViewBag.CategoryID = new SelectList(listCategory, "CateID", "CateName", selectedID);
        }

        #endregion ActionResult

        #region JsonResult

        public JsonResult ChangeStatus(int id)
        {
            bool result = false;
            try
            {
                result = ProductDao.Instance.changeStatus(id);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            var result = ProductDao.Instance.delete(id);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadData(int page, int pageSize, string search)
        {
            var products = string.IsNullOrEmpty(search) ? ProductDao.Instance.getObjectList("") : ProductDao.Instance.getObjectList(search);
            var data = Mapper.Map<List<ProductRequestDto>>(products);
            var response = data.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                data = response,
                total = data.Count,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeImage(int id, string imageUrl)
        {
            bool result = false;
            try
            {
                result = ProductDao.Instance.changeImage(id, imageUrl);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion JsonResult
    }
}