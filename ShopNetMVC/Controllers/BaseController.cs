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
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "User", action = "login" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}