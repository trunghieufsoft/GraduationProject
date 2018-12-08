using Models.Common;
using Models.DataAccess;
using Models.DataAccess.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ShopNetMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult UserLogin()
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
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
    }
}