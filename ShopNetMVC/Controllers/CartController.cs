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
            ViewBag.UserSession = session != null ? true : false;

            // Get Session order
            var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
            ViewBag.SessionCart = orders != null ? orders.Count : 0;

            ViewBag.IsOrdered = (List<OrderRequestDto>)Session[Constants.CART_SESSION] != null;

            var related = ProductDao.Instance.RelatedProducts(4);

            ViewBag.Related = Mapper.Map<List<ProductRequestDto>>(related);

            var listPrice = new List<string>();
            foreach (var item in related)
                listPrice.Add(Converter.formatPrice(item.Cost));
            ViewBag.listPrice = listPrice;
            ViewBag.Length = listPrice.Count;
            
            return View();
        }
        // POST: add cart
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

                return Json(new { result = true, message = "Thêm vào giỏ hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
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
                
                return Json(new
                {
                    orders,
                    totalPrice,
                    totalRows,
                    result = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, result = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckOut()
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            var user = new User();
            if (session != null)
            {
                user = UserDao.Instance.getByID(session.UserName);
            }
            ViewBag.UserSession = session != null ? true : false;

            var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
            var totalPrice = orders.Sum(o => o.Count * o.Product.Cost);

            var model = new BillRequestDto()
            {
                TotalPrice = totalPrice + 15000,
                DeliveryAddress = user.Address,
                CustomerName = user.FullName,
                Phone = user.Phone
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CheckOut(BillRequestDto model)
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            ViewBag.UserSession = session != null ? true : false;
            if (ModelState.IsValid)
            {
                var bill = Mapper.Map<Bill>(model);
                bill.UserID = ((UserSession)Session[Constants.USER_SESSION]).UserName;
                bill.TotalPrice = ((List<OrderRequestDto>)Session[Constants.CART_SESSION]).Sum(o => o.Count * o.Product.Cost) + 30000;
                BillDao.Instance.insert(bill);
                Session[Constants.CART_SESSION] = null;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Thêm đơn đặt hàng thất bại");
            }
            return View(model);
        }

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
                return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateOrder(int id, int count)
        {
            try
            {
                var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];

                orders.Find(o => o.ProdID == id).Count = count;
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, result = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}