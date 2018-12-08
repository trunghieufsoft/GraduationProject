using AutoMapper;
using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Models.Common.Encode;

namespace ShopNetMVC.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(string code)
        {
            var product = ProductDao.Instance.GetByCode(code);

            var model = Mapper.Map<ProductRequestDto>(product);

            ViewBag.ProductName = model.ProdName;
            ViewBag.Price = Converter.formatPrice(model.Cost);
            ViewBag.CateName = CategoryDao.Instance.getByID(model.CateID).CateName;

            var related = ProductDao.Instance.RelatedProducts(product.ProdID, 4);

            ViewBag.Related = Mapper.Map<List<ProductRequestDto>>(related);

            var listPrice = new List<string>();
            foreach (var item in related)
                listPrice.Add(Converter.formatPrice(item.Cost));
            ViewBag.listPrice = listPrice;
            ViewBag.Length = listPrice.Count;

            return View(model);
        }

        public JsonResult HomeProductsPaging(int page, int pageSize)
        {
            try
            {
                int totalRows, totalPages;
                var products = ProductDao.Instance.HomeProductsPaging(page, pageSize, out totalPages, out totalRows);

                var data = Mapper.Map<List<ProductRequestDto>>(products);
                return Json(new
                {
                    data,
                    totalRows,
                    totalPages,
                    result = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProduct(int id)
        {
            try
            {
                var product = ProductDao.Instance.getByID(id);
                var model = Mapper.Map<ProductRequestDto>(product);
                return Json(
                    new { data = model, status = true },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                return Json(
                    new { message = ex.Message, status = false }, JsonRequestBehavior.AllowGet
                    );
            }
        }
    }
}