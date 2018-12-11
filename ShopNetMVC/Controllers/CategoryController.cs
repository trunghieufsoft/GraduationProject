using AutoMapper;
using Models.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;
using Models.Common.Encode;
using Models.DataAccess.Dto;
using Models.Common;

namespace ShopNetMVC.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index(string code)
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            ViewBag.UserSession = session != null ? true : false;
            var products = ProductDao.Instance.GetByCategoryCode(code);

            // Get Session order
            var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
            ViewBag.SessionCart = orders != null ? orders.Count : 0;

            ViewBag.ProductCount = products.Count;

            var listPrice = new List<string>();
            foreach (var item in products)
                listPrice.Add(Converter.formatPrice(item.Cost));
            ViewBag.listPrice = listPrice;
            ViewBag.Length = products.Count;

            ViewBag.Category = CategoryDao.Instance.getByCode(code);
            ViewBag.Model = Mapper.Map<List<ProductRequestDto>>(products);

            return View();
        }

        [ChildActionOnly]
        public ActionResult RightCategory()
        {
            var cateCounts = ProductDao.Instance.GetProductCount();
            ViewBag.CateCount = cateCounts;
            var listCate = CategoryDao.Instance.GetListCategory();
            ViewBag.ListCate = listCate;
            ViewBag.Length = listCate.Count;
            return PartialView("_RightBarCategory");
        }
    }
}