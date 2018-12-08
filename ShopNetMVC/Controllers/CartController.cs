using AutoMapper;
using Models.Common;
using Models.DataAccess;
using Models.DataAccess.Dto;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShopNetMVC.Controllers
{
    public class CartController : BaseController
    {
        // GET: Cart
        public ActionResult Index()
        {
            ViewBag.IsOrdered = (List<OrderRequestDto>)Session[Constants.CART_SESSION] != null;
            return View();
        }

        public ActionResult CheckOut()
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }

            var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
            var user = UserDao.Instance.getByID(session.UserName);
            var totalPrice = orders.Sum(o => o.Count * o.Product.Cost);

            var model = new BillRequestDto()
            {
                TotalPrice = totalPrice + 30000,
                DeliveryAddress = user.Address,
                CustomerName = user.FullName,
                Phone = user.Phone
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CheckOut(BillRequestDto model)
        {
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

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public JsonResult AddToCart(int productId, int amount)
        {
            try
            {
                List<OrderRequestDto> orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];

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

        public JsonResult GetOrders(int page, int pageSize = 5)
        {
            try
            {
                var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
                var totalRows = orders.Count;
                var totalPages = (int)Math.Ceiling((double)totalRows / pageSize);

                var result = orders.Skip((page - 1) * pageSize).Take(pageSize);

                var totalPrice = orders.Sum(o => o.Count * o.Product.Cost);

                return Json(new
                {
                    data = result,
                    totalRows,
                    totalPages,
                    totalPrice,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
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