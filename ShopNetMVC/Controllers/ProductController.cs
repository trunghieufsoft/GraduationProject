using AutoMapper;
using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Models.Common.Encode;
using Models.Common;
using Models.DataAccess.Dto;

namespace ShopNetMVC.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            ViewBag.UserSession = session != null ? true : false;
            // Get Session order
            var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
            ViewBag.SessionCart = orders != null ? orders.Count : 0;
            return View();
        }

        public ActionResult Detail(string code = null)
        {
            if (code == null)
            {
                return RedirectToAction("Home");
            }
            var session = (UserSession)Session[Constants.USER_SESSION];
            ViewBag.UserSession = session != null ? true : false;
            // Get Session order
            var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
            ViewBag.SessionCart = orders != null ? orders.Count : 0;

            var product = ProductDao.Instance.GetByCode(code);

            var model = Mapper.Map<ProductRequestDto>(product);

            ViewBag.ProductName = model.ProdName;
            ViewBag.Price = Converter.formatPrice(model.Cost);
            ViewBag.CateName = CategoryDao.Instance.getByID(model.CateID).CateName;

            //var related = ProductDao.Instance.RelatedProducts();
            var related = ProductDao.Instance.Recommendations(4, true, product.CateID);

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

        [HttpPost]
        public JsonResult GetProduct(int id)
        {
            try
            {
                var product = ProductDao.Instance.getByID(id);
                var model = Mapper.Map<ProductRequestDto>(product);
                return Json(
                    new { data = model, status = true }
                );
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, status = false });
            }
        }
    }
}