using Models.Common;
using Models.DataAccess.Dto;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopNetMVC.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            ViewBag.UserSession = session != null ? true : false;
            base.OnActionExecuting(filterContext);
        }
    }
}