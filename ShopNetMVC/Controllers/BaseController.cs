using Models.Common;
using Models.DataAccess;
using Models.DataAccess.Dto;
using System.Web.Mvc;

namespace ShopNetMVC.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            ViewBag.UserSession = session != null ? true : false;
            var cateCounts = ProductDao.Instance.GetProductCount();
            ViewBag.CateCount = cateCounts;
            var listCate = CategoryDao.Instance.GetListCategory();
            ViewBag.ListCate = listCate;
            ViewBag.Count = listCate.Count;
            base.OnActionExecuting(filterContext);
        }
    }
}