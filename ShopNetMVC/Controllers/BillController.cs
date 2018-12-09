using AutoMapper;
using Models.Common;
using Models.DataAccess;
using Models.DataAccess.Dto;
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
            ViewBag.UserSession = session != null ? true : false;
            ViewBag.IsUser = ((UserSession)Session[Constants.USER_SESSION]).GrantID == (int)Constants.GrantID.User;
            return View();
        }

        public JsonResult GetBills(int pageIndex, int pageSize)
        {
            var user = ((UserSession)Session[Constants.USER_SESSION]);
            var isStaff = user.GrantID == (int)Constants.GrantID.Staff;
            var bills = BillDao.Instance.GetBills(user.UserName, isStaff);
            if (bills == null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }

            bills = bills.Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();
            int totalRows = bills.Count();
            int totalPages = (int)Math.Ceiling((double)totalRows / pageSize);
            var model = Mapper.Map<List<BillRequestDto>>(bills);

            return Json(new
            {
                status = true,
                totalRows,
                totalPages,
                data = model
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(string billId)
        {
            var result = BillDao.Instance.ChangeStatus(billId);
            return Json(new
            {
                result
            },
             JsonRequestBehavior.AllowGet);
        }
    }
}