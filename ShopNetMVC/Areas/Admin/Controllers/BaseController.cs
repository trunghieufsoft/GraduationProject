using Models.Common;
using Models.DataAccess.Dto;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopNetMVC.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserSession)Session[Constants.USER_SESSION];
            if (session == null || session.GrantID == (int)Constants.GrantID.User || session.GrantID == (int)Constants.GrantID.Staff)
            {
                filterContext.Result = new RedirectToRouteResult(new
                   RouteValueDictionary(new { Controller = "Login", Action = "Index", Area = "Admin" }));
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}