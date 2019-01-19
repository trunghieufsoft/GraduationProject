using AutoMapper;
using Models.Common;
using Models.Common.Encode;
using Models.DataAccess;
using Models.DataAccess.Dto;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShopNetMVC.Controllers
{
    public class BillController : BaseController
    {
        // GET: Bill
        public ActionResult Index()
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            var isLogin = session != null;
            if (!isLogin)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.UserSession = isLogin;
            ViewBag.IsUser = ((UserSession)Session[Constants.USER_SESSION]).GrantID == (int)Constants.GrantID.User;
            
            var related = ProductDao.Instance.Recommendations();

            ViewBag.Related = Mapper.Map<List<ProductRequestDto>>(related);

            var listPrice = new List<string>();
            foreach (var item in related)
                listPrice.Add(Converter.formatPrice(item.Cost));
            ViewBag.listPrice = listPrice;
            ViewBag.Length = listPrice.Count;

            return View();
        }

        [HttpPost]
        public JsonResult BillDetails(string billId)
        {
            var products = BillDao.Instance.BillDetails(billId);
            var data = Mapper.Map<List<ProductRequestDto>>(products);
            var count = BillDao.Instance.ProductCountDetails(billId);
            return Json(new { data, count });
        }


        public JsonResult GetBills(int pageIndex, int pageSize)
        {
            var user = ((UserSession)Session[Constants.USER_SESSION]);
            var isStaff = user.GrantID == (int)Constants.GrantID.Staff || user.GrantID == (int)Constants.GrantID.Manager;
            var bills = BillDao.Instance.GetBills(user.UserID, isStaff);
            if (bills == null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }

            int totalRows = bills.Count();
            bills = bills.Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();
            int totalPages = (int)Math.Ceiling((double)totalRows / pageSize);
            var model = Mapper.Map<List<BillRequestDto>>(bills);

            return Json(new
            {
                status = true,
                totalRows,
                totalPages,
                pageIndex,
                pageSize,
                data = model,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangeStatus(string billId)
        {
            var result = BillDao.Instance.ChangeStatus(billId);
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public JsonResult GetAccountLogin()
        {
            var user = ((UserSession)Session[Constants.USER_SESSION]);
            var info_user = user != null ? UserDao.Instance.getByID(user.UserID) : null;
            return Json(new { info = Mapper.Map<UserRequestDto>(info_user), status = true });
        }

        [HttpPost]
        public JsonResult AddToBill(string userid, string cusName, string address, string phone, string note, int pay)
        {
            try
            {
                var bill = new Bill()
                {
                    UserID = userid,
                    CustomerName = cusName,
                    DeliveryAddress = address,
                    Phone = phone,
                    Note = note,
                    TotalPrice = pay
                };
                var result = BillDao.Instance.insert(bill);
                return Json(new { result = true, billID = result });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public JsonResult AddToOrder(string request)
        {
            try
            {
                List<OrderRequestDto> orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
                var itemSource = new List<Order>();
                foreach (var item in orders)
                {
                    var orderItem = new Order()
                    {
                        BillID = request,
                        ProdID = item.ProdID,
                        Count = item.Count,
                    };
                    itemSource.Add(orderItem);
                }

                var result = OrderDao.Instance.insert(itemSource);
                if (result)
                {
                    Session.Remove(Constants.CART_SESSION);
                    Session[Constants.CART_SESSION] = null;
                }
                return Json(new { result = result });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = false,
                    message = ex.Message
                });
            }
        }
    }
}