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
            ViewBag.UserSession = session != null;
            ViewBag.UserName = session == null ? string.Empty : session.UserName;
            return PartialView("_Loginbar");
        }
    }
}