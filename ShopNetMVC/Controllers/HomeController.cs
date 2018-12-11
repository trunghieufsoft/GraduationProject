using Models.Common;
using Models.DataAccess;
using Models.DataAccess.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ShopNetMVC.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            // Get Session order
            var orders = (List<OrderRequestDto>)Session[Constants.CART_SESSION];
            ViewBag.SessionCart = orders != null ? orders.Count : 0;
            return View();
        }

        [ChildActionOnly]
        public ActionResult UserLogin()
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            ViewBag.UserSession = session != null ? true : false;
            ViewBag.UserName = session == null ? string.Empty : session.UserName;
            return PartialView("_Loginbar");
        }


        [ChildActionOnly]
        public ActionResult Category()
        {
            var categories = CategoryDao.Instance.GetListCategory();
            
            ViewBag.Categories = categories;

            return PartialView("_Category");
        }



        [ChildActionOnly]
        public ActionResult Bill()
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            ViewBag.IsLogin = session != null;
            return PartialView("_Bill");
        }

        public ActionResult Error()
        {
            ViewBag.Error = true;
            ViewBag.Message = "Lỗi không xác định 404";
            return PartialView("_Error");
        }
    }
}