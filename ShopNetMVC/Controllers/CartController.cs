using AutoMapper;
using Models.Common;
using Models.DataAccess;
using Models.DataAccess.Dto;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Models.Common.Encode;

namespace ShopNetMVC.Controllers
{
    public class CartController : BaseController
    {
        // GET: Cart
        public ActionResult Index()
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            ViewBag.LoginSuccess = session != null;

            // Get Session order
            var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
            ViewBag.SessionCart = orders != null ? orders.Count : 0;

            ViewBag.IsOrdered = (List<OrderRequestDto>)Session[Constants.CART_SESSION] != null;

            //var related = ProductDao.Instance.RelatedProducts(4);
            var related = ProductDao.Instance.Recommendations();

            ViewBag.Related = Mapper.Map<List<ProductRequestDto>>(related);

            var listPrice = new List<string>();
            foreach (var item in related)
                listPrice.Add(Converter.formatPrice(item.Cost));
            ViewBag.listPrice = listPrice;
            ViewBag.Length = listPrice.Count;
            
            var totalPrice = orders != null ? orders.Sum(o => o.Count * o.Product.Cost) : 0;
            var fee = totalPrice > 150000 ? totalPrice > 300000 ? 30000 : 15000 : 0;
            ViewBag.Pay = totalPrice + fee;

            return View();
        }
        // POST: add cart
        [HttpPost]
        public JsonResult AddToCart(int productId, int amount)
        {
            try
            {
                List<OrderRequestDto> orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
                if (orders == null)
                {
                    orders = new List<OrderRequestDto>();
                }

                var product = Mapper.Map<ProductRequestDto>(ProductDao.Instance.getByID(productId));

                var order = new OrderRequestDto() { ProdID = productId, Count = amount, Product = product };
                if (orders != null)
                {
                    var existed = orders.Find(o => o.ProdID == order.ProdID);

                    if (existed == null)
                    {
                        orders.Add(order);
                    }
                    else
                    {
                        orders.Find(o => o.ProdID == order.ProdID).Count += order.Count;
                    }

                    Session[Constants.CART_SESSION] = orders;
                }
                else
                {
                    orders = new List<OrderRequestDto>
                    {
                        order
                    };
                    Session[Constants.CART_SESSION] = orders;
                }

                return Json(new { result = true, message = "Thêm vào giỏ hàng thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        public JsonResult GetOrders(int page, int pageSize)
        {
            try
            {
                // Get session user
                var session = (UserSession)Session[Constants.USER_SESSION];
                var user = new User();
                if (session != null)
                {
                    user = UserDao.Instance.getByID(session.UserName);
                }
                // Get Session order
                var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];

                if (orders == null)
                {
                    orders = new List<OrderRequestDto>();
                }

                var totalPrice = orders.Sum(o => o.Count * o.Product.Cost);

                double totalRows = orders.Count();

                var response = orders.Skip((page - 1) * pageSize).Take(pageSize);

                return Json(new
                {
                    data = response,
                    totalPrice,
                    totalRows,
                    page,
                    pageSize,
                    result = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteOrder(int id)
        {
            try
            {
                var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
                orders.Remove(orders.Find(o => o.ProdID == id));
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateOrder(int id, int count)
        {
            try
            {
                var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];

                orders.Find(o => o.ProdID == id).Count = count;
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, result = false });
            }
        }

        public ActionResult PayOrders()
        {
            var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];

            if (orders == null)
            {
                orders = new List<OrderRequestDto>();
            }

            var totalPrice = orders.Sum(o => o.Count * o.Product.Cost);
            
            ViewBag._Price = Converter.formatPrice(totalPrice);
            var count = 0;
            foreach(var item in orders)
            {
                if (item.Count > 0)
                {
                    count += item.Count;
                }
            }
            ViewBag._Count = count.ToString() + (orders.Count >= 0 ? " SP" : "");
            var fee = totalPrice > 150000 ? totalPrice > 300000 ? 30000 : 15000 : 0;
            ViewBag._fee = Converter.formatPrice(fee);
            ViewBag._Pay = Converter.formatPrice(totalPrice + fee);
            return PartialView("_PayOrders");
        }
    }
}